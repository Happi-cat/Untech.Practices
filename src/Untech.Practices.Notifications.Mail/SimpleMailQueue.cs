using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using RazorEngine;
using RazorEngine.Templating;

namespace Untech.Practices.Notifications.Mail
{
	public class SimpleMailQueue : INotificationQueue<Mail>, IRealtimeNotifier<Mail>
	{
		private readonly ITemplateManager _templateManager;
		private readonly SimpleMailQueueOptions _options;

		public SimpleMailQueue(ITemplateManager templateManager, SimpleMailQueueOptions options)
		{
			_templateManager = templateManager ?? throw new ArgumentNullException(nameof(templateManager));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		public Task EnqueueAsync(Mail notification)
		{
			return Task.Run(() => SendAsync(notification));
		}

		public Task EnqueueAsync(IEnumerable<Mail> notifications)
		{
			return Task.Run(() => SendAsync(notifications));
		}

		public async Task SendAsync(Mail notification, CancellationToken cancellationToken = default)
		{
			var message = CreateMimeMessage(notification);

			using (var client = new SmtpClient())
			{
				await client.ConnectAsync(_options.Host, _options.Port, _options.UseSsl, cancellationToken);
				await client.SendAsync(message, cancellationToken);
				await client.DisconnectAsync(true, cancellationToken);
			}
		}

		public async Task SendAsync(IEnumerable<Mail> notifications, CancellationToken cancellationToken = default)
		{
			var messages = notifications.Select(CreateMimeMessage).ToList();

			using (var client = new SmtpClient())
			{
				await client.ConnectAsync(_options.Host, _options.Port, _options.UseSsl, cancellationToken);
				foreach (MimeMessage message in messages)
					await client.SendAsync(message, cancellationToken);
				await client.DisconnectAsync(true, cancellationToken);
			}
		}

		private MimeMessage CreateMimeMessage(Mail mail)
		{
			var modelType = mail.Payload.GetType();

			var subject = CompileTemplate(mail.TemplateKey + ".subject", modelType, mail.Payload);
			var htmlBody = CompileTemplate(mail.TemplateKey + ".body.html", modelType, mail.Payload);
			var txtBody = CompileTemplate(mail.TemplateKey + ".body.txt", modelType, mail.Payload);

			var message = new MimeMessage();

			message.From.Add(ToMimekitAddress(mail.From ?? _options.From));
			message.To.AddRange(ToMimeKitAddresses(mail.To));
			message.Cc.AddRange(ToMimeKitAddresses(mail.Cc));
			message.Bcc.AddRange(ToMimeKitAddresses(mail.Bcc));

			message.Subject = subject;
			message.Body = new Multipart("alternative")
			{
				new TextPart(MimeKit.Text.TextFormat.Plain) { Text = txtBody },
				new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlBody }
			};
			return message;
		}

		private string CompileTemplate(string templateKey, Type modelType, object args)
		{
			var templateSource = _templateManager.Resolve(new NameOnlyTemplateKey(templateKey, ResolveType.Global, null));

			return Engine.Razor.RunCompile(templateSource, templateKey, modelType, args);
		}

		private static MimeKit.MailboxAddress ToMimekitAddress(MailboxAddress mailboxAddress)
		{
			return new MimeKit.MailboxAddress(mailboxAddress.DisplayName, mailboxAddress.Email);
		}

		private static IEnumerable<MimeKit.MailboxAddress> ToMimeKitAddresses(
			IEnumerable<MailboxAddress> mailboxAddresses)
		{
			return mailboxAddresses == null
				? Enumerable.Empty<MimeKit.MailboxAddress>()
				: mailboxAddresses.Select(ToMimekitAddress);
		}
	}
}
