using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Untech.Practices.Notifications.Mail
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

		/// <summary>
		/// Initializes a new isntance of the <see cref="Mail"/> with a predefined recepient address and arguments.
		/// </summary>
		/// <param name="to">The recepient address.</param>
		/// <param name="arguments">The mail arguments.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="to"/> or <paramref name="arguments"/> is null.
		/// </exception>
		public Mail(MailboxAddress to, IMailArguments arguments)
		{
			To = new List<MailboxAddress>
			{
				to ?? throw new ArgumentNullException(nameof(to))
			};
			Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
		}

		/// <summary>
		/// Initializes a new isntance of the <see cref="Mail"/> with a predefined recepients addresses and arguments.
		/// </summary>
		/// <param name="to">Recepients addresses.</param>
		/// <param name="arguments">The mail arguments.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="to"/> or <paramref name="arguments"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException"><paramref name="to"/> is empty.</exception>
		public Mail(IEnumerable<MailboxAddress> to, IMailArguments arguments)
		{
			var toList = to?.ToList() ?? throw new ArgumentNullException(nameof(to));
			if (toList.Count == 0) throw new ArgumentException("Cannot have zero elements", nameof(to));

			To = toList;
			Arguments = arguments;
		}

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
		/// Gets additional mail arguments.
		/// </summary>
		[DataMember]
		public IMailArguments Arguments { get; private set; }
	}
}
