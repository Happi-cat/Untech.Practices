using System;
using System.Runtime.Serialization;

namespace Untech.Practices.Localization
{
	[DataContract]
	public struct LocalizedString : ILocalizableString, IEquatable<LocalizedString>
	{
		public LocalizedString(string value)
		{
			Value = value;
		}

		public static explicit operator LocalizedString(string value)
		{
			return new LocalizedString(value);
		}

		public static implicit operator string(LocalizedString localizedString)
		{
			return localizedString.Value;
		}

		[DataMember]
		public string Value { get; }

		public string Localize(ILocalizationContext context)
		{
			return Value;
		}

		public string Localize(ILocalizationContext context, params object[] args)
		{
			return string.Format(Value, args);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			return obj is LocalizableString other && Equals(other);
		}

		public bool Equals(LocalizedString other)
		{
			return string.Equals(Value, other.Value, StringComparison.Ordinal);
		}

		public override int GetHashCode()
		{
			return Tuple.Create(Value).GetHashCode();
		}

		public override string ToString()
		{
			return Value;
		}
	}
}