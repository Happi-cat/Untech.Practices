using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Untech.Practices.Mail
{
	/// <summary>
	/// Represents mail information that can be used for rendering and sending.
	/// </summary>
	[DataContract]
	public class Mail
	{
		/// <summary>
		/// For serializers
		/// </summary>
		private Mail()
		{
		}

		public Mail(string type, MailboxAddress to)
			: this(type, new List<MailboxAddress> {to})
		{
		}

		public Mail(string type, IEnumerable<MailboxAddress> to)
		{
			if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException(nameof(type));

			var toList = to?.ToList() ?? throw new ArgumentNullException(nameof(to));
			if (toList.Count == 0) throw new ArgumentException(nameof(to));

			To = toList;
		}

		/// <summary>
		/// Gets type of mail to process.
		/// </summary>
		[DataMember]
		public string Type { get; private set; }

		/// <summary>
		/// Gets or sets optional sender mail address.
		/// </summary>
		[DataMember]
		public MailboxAddress From { get; set; }

		/// <summary>
		/// Gets recepients mail addresses.
		/// </summary>
		[DataMember]
		public IReadOnlyCollection<MailboxAddress> To { get; private set; }

		/// <summary>
		/// Gets or sets optional CC mail adressess.
		/// </summary>
		[DataMember]
		public IReadOnlyCollection<MailboxAddress> Cc { get; set; }

		/// <summary>
		/// Gets or sets optional BCC mail addresses.
		/// </summary>
		[DataMember]
		public IReadOnlyCollection<MailboxAddress> Bcc { get; set; }

		/// <summary>
		/// Gets or sets additional mail arguments.
		/// </summary>
		[DataMember]
		public object Arguments { get; set; }
	}
}