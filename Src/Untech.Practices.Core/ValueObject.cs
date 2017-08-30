using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="System.IEquatable{Untech.Practices.ValueObject}" />
	public abstract class ValueObject : IEquatable<ValueObject>
	{
		public override string ToString()
		{
			var props = string.Join(", ", GetEquatableProperties());
			return string.Concat("(", props, ")");
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ValueObject);
		}

		public bool Equals(ValueObject other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(other, null)) return false;

			return GetType() == other.GetType() && GetEquatableProperties().SequenceEqual(other.GetEquatableProperties());
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 37 + GetType().GetHashCode();
				foreach (var prop in GetEquatableProperties())
				{
					hash = hash * 37 + prop?.GetHashCode() ?? 0;
				}
				return hash;
			}
		}

		/// <summary>
		/// Gets the equatable properties. This method should return values of immutable properties and fields.
		/// </summary>
		/// <returns></returns>
		protected abstract IEnumerable<object> GetEquatableProperties();
	}
}