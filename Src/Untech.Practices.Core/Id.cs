using System;
using System.Collections.Generic;

namespace Untech.Practices
{
	public abstract class Id<T> : IEquatable<Id<T>>
		where T: IEquatable<T>
	{
		private readonly T _value;

		protected Id(T value) => _value = value;

		public static implicit operator T(Id<T> id)
		{
			return id == null ? default(T) : id._value;
		}

		public override string ToString() => $"{_value}";

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 37 + EqualityComparer<T>.Default.GetHashCode(_value);
				return hash;
			}
		}

		public bool Equals(Id<T> other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return EqualityComparer<T>.Default.Equals(_value, other._value);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj is Id<T> self) return Equals(self);
			return false;
		}
	}
}