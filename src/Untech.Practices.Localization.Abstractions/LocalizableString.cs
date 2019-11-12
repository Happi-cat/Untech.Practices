using System;
using System.Runtime.Serialization;

namespace Untech.Practices.Localization
{
	[DataContract]
	public struct LocalizableString : ILocalizableString, IEquatable<LocalizableString>
	{
		public LocalizableString(string key, string partitionKey)
		{
			Key = key;
			PartitionKey = partitionKey;
		}

		[DataMember]
		public string Key { get; }

		[DataMember]
		public string PartitionKey { get; }

		public string Localize(ILocalizationContext context)
		{
			return context.GetPartition(PartitionKey).GetString(Key);
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
			return string.Equals(Key, other.Key, StringComparison.Ordinal)
				&& string.Equals(PartitionKey , other.PartitionKey, StringComparison.OrdinalIgnoreCase);
		}

		public override int GetHashCode()
		{
			return Tuple.Create(Key, PartitionKey.ToLower()).GetHashCode();
		}

		public override string ToString()
		{
			return $"[l18n:{PartitionKey}:{Key}]";
		}
	}
}