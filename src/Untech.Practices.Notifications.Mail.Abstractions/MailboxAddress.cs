using System;
using System.Runtime.Serialization;

namespace Untech.Practices.Notifications.Mail
{
	/// <summary>
	///     Represents mail address.
	/// </summary>
	[DataContract]
	public class MailboxAddress : IEquatable<MailboxAddress>
	{
		/// <summary>
		///     For serializers
		/// </summary>
		private MailboxAddress()
		{
		}

		/// <summary>
		///     Initializes a new instance of <see cref="MailboxAddress" /> with the specified <paramref name="email" />.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="email" /> is null or whitespace.
		/// </exception>
		public MailboxAddress(string email)
		{
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

			Email = email;
		}

		/// <summary>
		///     Gets email.
		/// </summary>
		[DataMember]
		public string Email { get; private set; }

		/// <summary>
		///     Gets or sets display name if it was specified, otherwise returns null.
		/// </summary>
		[DataMember]
		public string DisplayName { get; set; }

		public bool Equals(MailboxAddress other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Equals(Email, other.Email, StringComparison.OrdinalIgnoreCase);
		}

		public static implicit operator MailboxAddress(string email)
		{
			return new MailboxAddress(email);
		}

		public static implicit operator string(MailboxAddress address)
		{
			return address?.Email;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return string.IsNullOrWhiteSpace(DisplayName)
				? Email
				: $"{DisplayName} <{Email}>";
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((MailboxAddress)obj);
		}

		public override int GetHashCode()
		{
			return StringComparer.OrdinalIgnoreCase.GetHashCode(Email);
		}
	}
}