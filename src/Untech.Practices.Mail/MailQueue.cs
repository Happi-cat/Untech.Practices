using System;
using System.Collections.Generic;
using System.Linq;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RazorEngine;
using RazorEngine.Templating;

namespace Untech.Practices.Mail
{
	public class MailQueueOptions
	{
		public MailboxAddress From { get; private set; }
		public string Host { get; private set; }
		public int Port { get; private set; }
		public bool UseSsl { get; set; }
	}

	public class MailQueue : IMailQueue
	{
		private readonly ITemplateManager _templateManager;
		private readonly MailQueueOptions _options;

		public MailQueue(ITemplateManager templateManager, MailQueueOptions options)
		{
			_templateManager = templateManager ?? throw new ArgumentNullException(nameof(templateManager));
			_options = options;
		}

		public void Enqueue(Mail mail)
		{
			var message = CreateMimeMessage(mail);

			using (var client = new SmtpClient())
			{
				client.Connect(_options.Host, _options.Port, _options.UseSsl);
				client.Send(message);
				client.Disconnect(true);
			}
		}

		private MimeMessage CreateMimeMessage(Mail mail)
		{
			var modelType = mail.Arguments.GetType();

			var subject = CompileTemplate(mail.Arguments.Type + ".subject", modelType, mail.Arguments);
			var htmlBody = CompileTemplate(mail.Arguments.Type + ".body.html", modelType, mail.Arguments);
			var txtBody = CompileTemplate(mail.Arguments.Type + ".body.txt", modelType, mail.Arguments);

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

		private string CompileTemplate(string templateKey, Type modelType, IMailArguments args)
		{
			var templateSource = _templateManager.Resolve(new NameOnlyTemplateKey(args.Type, ResolveType.Global, null));

			return Engine.Razor.RunCompile(templateSource, templateKey, modelType, args);
		}

		private static MimeKit.MailboxAddress ToMimekitAddress(MailboxAddress mailboxAddress)
		{
			return new MimeKit.MailboxAddress(mailboxAddress.DisplayName, mailboxAddress.Email);
		}

		private static IEnumerable<MimeKit.MailboxAddress> ToMimeKitAddresses(
			IEnumerable<MailboxAddress> mailboxAddresses)
		{
			if (mailboxAddresses == null) return Enumerable.Empty<MimeKit.MailboxAddress>();
			return mailboxAddresses.Select(ToMimekitAddress);
		}
	}
}