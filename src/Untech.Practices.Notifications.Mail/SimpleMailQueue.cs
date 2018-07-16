﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using RazorEngine;
using RazorEngine.Templating;

namespace Untech.Practices.Notifications.Mail
{
	public class SimpleMailQueue : IMailQueue
	{
		private readonly ITemplateManager _templateManager;
		private readonly SimpleMailQueueOptions _options;

		public SimpleMailQueue(ITemplateManager templateManager, SimpleMailQueueOptions options)
		{
			_templateManager = templateManager ?? throw new ArgumentNullException(nameof(templateManager));
			_options = options ?? throw new ArgumentNullException(nameof(options));
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
			var modelType = mail.TemplateArguments.GetType();

			var subject = CompileTemplate(mail.TemplateKey + ".subject", modelType, mail.TemplateArguments);
			var htmlBody = CompileTemplate(mail.TemplateKey + ".body.html", modelType, mail.TemplateArguments);
			var txtBody = CompileTemplate(mail.TemplateKey + ".body.txt", modelType, mail.TemplateArguments);

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

		private string CompileTemplate(string templateKey, Type modelType, IMailTemplateArguments args)
		{
			var templateSource = _templateManager.Resolve(new NameOnlyTemplateKey(args.TemplateKey, ResolveType.Global, null));

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