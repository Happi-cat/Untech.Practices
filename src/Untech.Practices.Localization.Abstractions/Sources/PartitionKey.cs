using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Untech.Practices.Localization.Sources
{
	public struct PartitionKey : IEquatable<PartitionKey>
	{
		public PartitionKey([CanBeNull]string key, [CanBeNull]CultureInfo culture)
		{
			Key = (key ?? "").ToLower();
			Culture = culture ?? CultureInfo.InvariantCulture;
		}

		[NotNull]
		public string Key { get; }

		[NotNull]
		public CultureInfo Culture { get; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			return obj is PartitionKey other && Equals(other);
		}

		public bool Equals(PartitionKey other)
		{
			return string.Equals(Key, other.Key, StringComparison.Ordinal)
				&& Equals(Culture, other.Culture);
		}

		public override int GetHashCode()
		{
			return Tuple.Create(Key, Culture).GetHashCode();
		}

		public override string ToString()
		{
			return $"({Key}, {Culture.Name})";
		}
	}
}