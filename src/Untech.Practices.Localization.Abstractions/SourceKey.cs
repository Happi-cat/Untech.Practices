using System;
using System.Globalization;

namespace Untech.Practices.Localization
{
	public struct SourceKey : IEquatable<SourceKey>
	{
		public SourceKey(string source, CultureInfo culture)
		{
			Source = (source ?? "").ToLower();
			Culture = culture ?? CultureInfo.InvariantCulture;
		}

		public string Source { get; }
		public CultureInfo Culture { get; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			return obj is SourceKey other && Equals(other);
		}

		public bool Equals(SourceKey other)
		{
			return Source == other.Source && Equals(Culture, other.Culture);
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