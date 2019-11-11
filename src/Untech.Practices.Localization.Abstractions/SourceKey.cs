using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Untech.Practices.Localization
{
	public struct SourceKey : IEquatable<SourceKey>
	{
		public SourceKey([CanBeNull]string source, [CanBeNull]CultureInfo culture)
		{
			Source = (source ?? "").ToLower();
			Culture = culture ?? CultureInfo.InvariantCulture;
		}

		[NotNull]
		public string Source { get; }

		[NotNull]
		public CultureInfo Culture { get; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			return obj is SourceKey other && Equals(other);
		}

		public bool Equals(SourceKey other)
		{
			return string.Equals(Source, other.Source, StringComparison.Ordinal)
				&& Equals(Culture, other.Culture);
		}

		public override int GetHashCode()
		{
			return Tuple.Create(Source, Culture).GetHashCode();
		}

		public override string ToString()
		{
			return $"({Source}, {Culture.Name})";
		}
	}
}