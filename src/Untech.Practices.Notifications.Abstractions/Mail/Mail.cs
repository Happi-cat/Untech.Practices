using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Untech.Practices.Notifications.Mail
{
	/// <summary>
	///     Represents mail information that can be used for rendering and sending.
	/// </summary>
	[DataContract]
	public class Mail : INotification
	{
		/// <summary>
		///     For serializers
		/// </summary>
		private Mail()
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Mail" /> with a predefined recipient address and arguments.
		/// </summary>
		/// <param name="to">The recipient address.</param>
		/// <param name="templateKey">The mail template key.</param>
		/// <param name="payload">The mail template arguments.</param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="to" /> or <paramref name="templateKey" /> is null.
		/// </exception>
		public Mail(MailboxAddress to, string templateKey, object payload = null)
		{
			To = new List<MailboxAddress>
			{
				to ?? throw new ArgumentNullException(nameof(to))
			};
			TemplateKey = templateKey ?? throw new ArgumentNullException(nameof(templateKey));
			Payload = payload;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Mail" /> with a predefined recipients addresses and arguments.
		/// </summary>
		/// <param name="to">Recipients addresses.</param>
		/// <param name="templateKey">The mail template key.</param>
		/// <param name="payload">The mail template arguments.</param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="to" /> or <paramref name="templateKey" /> is null.
		/// </exception>
		/// <exception cref="ArgumentException"><paramref name="to" /> is empty.</exception>
		public Mail(IEnumerable<MailboxAddress> to, string templateKey, object payload = null)
		{
			List<MailboxAddress> toList = to?.ToList() ?? throw new ArgumentNullException(nameof(to));
			if (toList.Count == 0)
				throw new ArgumentException("Cannot have zero elements", nameof(to));

			To = toList;
			TemplateKey = templateKey ?? throw new ArgumentNullException(nameof(templateKey));
			Payload = payload;
		}

		/// <summary>
		///     Gets or sets optional sender mail address.
		/// </summary>
		[DataMember]
		public MailboxAddress From { get; set; }

		/// <summary>
		///     Gets recipients mail addresses.
		/// </summary>
		[DataMember]
		public IReadOnlyCollection<MailboxAddress> To { get; private set; }

		/// <summary>
		///     Gets or sets optional CC mail addresses.
		/// </summary>
		[DataMember]
		public IReadOnlyCollection<MailboxAddress> Cc { get; set; }

		/// <summary>
		///     Gets or sets optional BCC mail addresses.
		/// </summary>
		[DataMember]
		public IReadOnlyCollection<MailboxAddress> Bcc { get; set; }

		/// <summary>
		///     Gets mail template key.
		/// </summary>
		[DataMember]
		public string TemplateKey { get; private set; }

		/// <summary>
		///     Gets additional mail arguments.
		/// </summary>
		[DataMember]
		public object Payload { get; private set; }
	}
}