using System;
using System.Runtime.Serialization;

namespace Untech.Practices.Localization
{
	[DataContract]
	public struct LocalizableString : ILocalizableString, IEquatable<LocalizableString>
	{
		public LocalizableString(string reference, string source)
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
			return context.GetSource(Source).GetString(Reference);
		}

		public string Localize(ILocalizationContext context, params object[] args)
		{
			return string.Format(Localize(context), args);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			return obj is LocalizableString other && Equals(other);
		}

		public bool Equals(LocalizableString other)
		{
			return string.Equals(Reference, other.Reference, StringComparison.Ordinal)
				&& string.Equals(Source , other.Source, StringComparison.OrdinalIgnoreCase);
		}

		public override int GetHashCode()
		{
			return Tuple.Create(Reference, Source.ToLower()).GetHashCode();
		}

		public override string ToString()
		{
			return $"[l18n:{Source}:{Reference}]";
		}
	}
}