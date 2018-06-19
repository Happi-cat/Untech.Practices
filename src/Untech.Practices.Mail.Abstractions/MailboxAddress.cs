using System;
using System.Runtime.Serialization;

namespace Untech.Practices.Mail
{
	[DataContract]
	public class MailboxAddress
	{
		private MailboxAddress()
		{
		}

		public MailboxAddress(string email)
		{
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

			Email = email;
		}

		[DataMember]
		public string Email { get; private set; }

		[DataMember]
		public string DisplayName { get; set; }

		public static implicit operator MailboxAddress(string email)
		{
			return new MailboxAddress(email);
		}
	}
}