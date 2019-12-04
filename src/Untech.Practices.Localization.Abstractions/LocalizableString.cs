using System;
using System.Runtime.Serialization;

namespace Untech.Practices.Localization
{
	[DataContract]
	public struct LocalizableString : ILocalizableString, IEquatable<LocalizableString>
	{
		public LocalizableString(string name, string partition)
		{
			Name = name;
			Partition = partition;
		}

		[DataMember]
		public string Name { get; }

		[DataMember]
		public string Partition { get; }

		public string Localize(ILocalizationContext context)
		{
			return context.Localize(Partition, Name);
		}

		public string Localize(ILocalizationContext context, params object[] args)
		{
			return context.Localize(Partition, Name, args);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			return obj is LocalizableString other && Equals(other);
		}

		public bool Equals(LocalizableString other)
		{
			return string.Equals(Name, other.Name, StringComparison.Ordinal)
				&& string.Equals(Partition , other.Partition, StringComparison.OrdinalIgnoreCase);
		}

		public override int GetHashCode()
		{
			return Tuple.Create(Name, Partition.ToLower()).GetHashCode();
		}

		public override string ToString()
		{
			return $"[l10n:{Partition}:{Name}]";
		}
	}
}