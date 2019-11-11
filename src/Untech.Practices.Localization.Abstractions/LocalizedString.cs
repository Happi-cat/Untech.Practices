using System;
using System.Runtime.Serialization;

namespace Untech.Practices.Localization
{
	[DataContract]
	public struct LocalizedString : ILocalizableString, IEquatable<LocalizedString>
	{
		public LocalizedString(string translation)
		{
			Translation = translation;
		}

		[DataMember]
		public string Translation { get; }

		public string Localize(ILocalizationContext context)
		{
			return Translation;
		}

		public string Localize(ILocalizationContext context, params object[] args)
		{
			return string.Format(Translation, args);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			return obj is LocalizableString other && Equals(other);
		}

		public bool Equals(LocalizedString other)
		{
			return string.Equals(Translation, other.Translation, StringComparison.Ordinal);
		}

		public override int GetHashCode()
		{
			return Tuple.Create(Translation).GetHashCode();
		}

		public override string ToString()
		{
			return Translation;
		}
	}
}