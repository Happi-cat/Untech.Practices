using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices
{
	/// <summary>
	/// </summary>
	/// <seealso cref="System.IEquatable{Untech.Practices.ValueObject}" />
	public abstract class ValueObject<TSelf> : IEquatable<TSelf>
		where TSelf : ValueObject<TSelf>
	{
		public bool Equals(TSelf other)
		{
			if (ReferenceEquals(this, other))
				return true;
			if (ReferenceEquals(other, null))
				return false;

			using (IEnumerator<object> thisValues = GetEquatableProperties().GetEnumerator())
			using (IEnumerator<object> otherValues = other.GetEquatableProperties().GetEnumerator())
			{
				while (thisValues.MoveNext() && otherValues.MoveNext())
				{
					var thisValue = thisValues.Current;
					var otherValue = otherValues.Current;

					if (ReferenceEquals(thisValue, null) ^ ReferenceEquals(otherValue, null))
						return false;

					if (thisValue != null && !thisValue.Equals(otherValue))
						return false;
				}

				return !thisValues.MoveNext() && !otherValues.MoveNext();
			}
		}

		public override string ToString()
		{
			string props = string.Join(", ", GetEquatableProperties());
			return string.Concat("(", props, ")");
		}

		public static bool operator ==(ValueObject<TSelf> left, ValueObject<TSelf> right)
		{
			if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
				return false;
			return ReferenceEquals(left, null) || left.Equals(right);
		}

		public static bool operator !=(ValueObject<TSelf> left, ValueObject<TSelf> right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;
			if (ReferenceEquals(obj, null))
				return false;
			if (obj is TSelf self)
				return Equals(self);
			return false;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return GetEquatableProperties()
					.Aggregate(17, (current, prop) => current * 37 + prop?.GetHashCode() ?? 0);
			}
		}

		/// <summary>
		///     Gets the equatable properties. This method should return values of immutable properties and fields.
		/// </summary>
		/// <returns></returns>
		protected abstract IEnumerable<object> GetEquatableProperties();
	}
}