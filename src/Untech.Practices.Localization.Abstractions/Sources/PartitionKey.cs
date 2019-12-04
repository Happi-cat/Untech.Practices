using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Untech.Practices.Localization.Sources
{
	public struct PartitionKey : IEquatable<PartitionKey>
	{
		public PartitionKey([CanBeNull]string name, [CanBeNull]CultureInfo culture)
		{
			Name = (name ?? "").ToLower();
			Culture = culture ?? CultureInfo.InvariantCulture;
		}

		[NotNull]
		public string Name { get; }

		[NotNull]
		public CultureInfo Culture { get; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			return obj is PartitionKey other && Equals(other);
		}

		public bool Equals(PartitionKey other)
		{
			return string.Equals(Name, other.Name, StringComparison.Ordinal)
				&& Equals(Culture, other.Culture);
		}

		public override int GetHashCode()
		{
			return Tuple.Create(Name, Culture).GetHashCode();
		}

		public override string ToString()
		{
			return $"({Name}, {Culture.Name})";
		}
	}
}