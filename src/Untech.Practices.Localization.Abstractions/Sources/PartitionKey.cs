using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Untech.Practices.Localization.Sources
{
	public readonly struct PartitionKey : IEquatable<PartitionKey>
	{
		private readonly string _name;
		private readonly CultureInfo _culture;

		public PartitionKey([CanBeNull]string name, [CanBeNull]CultureInfo culture)
		{
			_name = (name ?? "").ToLower();
			_culture = culture ?? CultureInfo.InvariantCulture;
		}

		[NotNull]
		public string Name => _name ?? "";

		[NotNull]
		public CultureInfo Culture => _culture ?? CultureInfo.InvariantCulture;

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
			return Tuple.Create(Name, Culture).ToString();
		}
	}
}