using System;

namespace Untech.Practices.Monads
{
	public struct Maybe<T> : IEquatable<Maybe<T>>
	{
		public static readonly Maybe<T> Nothing = new Maybe<T>();

		public Maybe(T value)
		{
			HasValue = true;
			Value = value;
		}

		public bool HasValue { get; }
		public T Value { get; }

		public bool Equals(Maybe<T> other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(other, null)) return false;
			return HasValue == other.HasValue && Equals(Value, other.Value);
		}

		public override string ToString()
		{
			return HasValue ? "<" + Value + ">" : "<Nothing>";
		}
	}
}