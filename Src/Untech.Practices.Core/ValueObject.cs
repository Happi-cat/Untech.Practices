using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices
{
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

		protected abstract IEnumerable<object> GetEquatableProperties();
	}
}