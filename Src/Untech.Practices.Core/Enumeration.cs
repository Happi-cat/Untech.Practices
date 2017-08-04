using System;

namespace Untech.Practices
{
	public abstract class Enumeration : IComparable<Enumeration>, IEquatable<Enumeration>
	{
		protected Enumeration()
		{
		}

		protected Enumeration(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public int Id { get; }

		public string Name { get; }


		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Enumeration);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 37 + GetType().GetHashCode();
				hash = hash * 37 + Id;
				return hash;
			}
		}

		public int CompareTo(object other)
		{
			return CompareTo(other as Enumeration);
		}

		public bool Equals(Enumeration other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(other, null)) return false;

			return GetType() == other.GetType() && Id.Equals(other.Id);
		}

		public int CompareTo(Enumeration other)
		{
			if (ReferenceEquals(other, null)) throw new ArgumentException("Argument is null or has wrong type", nameof(other));
			return Id.CompareTo(other.Id);
		}
	}
}