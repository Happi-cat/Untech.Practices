using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices
{
	/// <summary>
	///
	/// </summary>
	/// <seealso cref="System.IEquatable{Untech.Practices.ValueObject}" />
	public abstract class ValueObject<TSelf> : IEquatable<TSelf>
		where TSelf : ValueObject<TSelf>
	{
		public override string ToString()
		{
			var props = string.Join(", ", GetEquatableProperties());
			return string.Concat("(", props, ")");
		}

		public static bool operator ==(ValueObject<TSelf> left, ValueObject<TSelf> right)
		{
			return ReferenceEquals(left, null)
				? ReferenceEquals(right, null)
				: left.Equals(right);
		}

		public static bool operator !=(ValueObject<TSelf> left, ValueObject<TSelf> right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as TSelf);
		}

		public bool Equals(TSelf other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(other, null)) return false;

			return GetEquatableProperties().SequenceEqual(other.GetEquatableProperties());
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return GetEquatableProperties().Aggregate(17, (current, prop) => current * 37 + prop?.GetHashCode() ?? 0);
			}
		}

		/// <summary>
		/// Gets the equatable properties. This method should return values of immutable properties and fields.
		/// </summary>
		/// <returns></returns>
		protected abstract IEnumerable<object> GetEquatableProperties();
	}
}