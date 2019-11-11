using System;
using System.Runtime.Serialization;

namespace Untech.Practices.Localization
{
	[DataContract]
	public struct LocalizedString : ILocalizableString, IEquatable<LocalizedString>
	{
		public LocalizedString(string reference, string source)
		{
			Reference = reference;
			Source = source;
		}

		[DataMember]
		public string Reference { get; }

		[DataMember]
		public string Source { get; }

		public string Localize(ILocalizationContext context)
		{
			return Reference;
		}

		public string Localize(ILocalizationContext context, params object[] args)
		{
			return string.Format(Reference, args);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			return obj is LocalizableString other && Equals(other);
		}

		public bool Equals(LocalizedString other)
		{
			return Reference == other.Reference;
		}

		public override int GetHashCode()
		{
			return Tuple.Create(Reference).GetHashCode();
		}

		public override string ToString()
		{
			return Reference;
		}
	}
}