using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Untech.Practices.Mail
{
	[DataContract]
	public class Mail<TArgs>
	{
		private Mail()
		{
		}

		public Mail(MailboxAddress from, MailboxAddress to)
			: this(from, new List<MailboxAddress> { to })
		{
		}

		public Mail(MailboxAddress from, IEnumerable<MailboxAddress> to)
		{
			var toList = to?.ToList() ?? throw new ArgumentNullException(nameof(to));
			if (toList.Count == 0) throw new ArgumentException(nameof(to));

			From = from ?? throw new ArgumentNullException(nameof(from));
			To = toList;
		}

		[DataMember]
		public MailboxAddress From { get; private set; }

		[DataMember]
		public IReadOnlyCollection<MailboxAddress> To { get; private set; }

		[DataMember]
		public IReadOnlyCollection<MailboxAddress> Cc { get; set; }

		[DataMember]
		public IReadOnlyCollection<MailboxAddress> Bcc { get; set; }

		[DataMember]
		public TArgs Arguments { get; set; }
	}
}