using System;
using System.Runtime.Serialization;

namespace Untech.Practices
{
	[DataContract]
	public abstract class Enumeration<TSelf> : IComparable<Enumeration<TSelf>>, IEquatable<Enumeration<TSelf>>
		where TSelf : Enumeration<TSelf>
	{
		protected Enumeration()
		{
		}

		protected Enumeration(int id, string name)
		{
			Id = id;
			Name = name;
		}

		[DataMember]
		public int Id { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		public override string ToString()
		{
			return string.Format("({0}, {1})", Id, Name);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Enumeration<TSelf>);
		}

		public bool Equals(Enumeration<TSelf> other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(other, null)) return false;

			return Id.Equals(other.Id);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 37 + Id;
				return hash;
			}
		}

		public int CompareTo(object other)
		{
			return CompareTo(other as Enumeration<TSelf>);
		}

		public int CompareTo(Enumeration<TSelf> other)
		{
			if (ReferenceEquals(other, null)) throw new ArgumentNullException(nameof(other));
			return Id.CompareTo(other.Id);
		}
	}

	[DataContract]
	public abstract class Enumeration<TSelf, TKey> : IComparable<Enumeration<TSelf, TKey>>, IEquatable<Enumeration<TSelf, TKey>>
		where TSelf : Enumeration<TSelf, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		protected Enumeration()
		{
		}

		protected Enumeration(TKey id, string name)
		{
			Id = id;
			Name = name;
		}

		[DataMember]
		public TKey Id { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		public override string ToString()
		{
			return string.Format("({0}, {1})", Id, Name);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Enumeration<TSelf, TKey>);
		}

		public bool Equals(Enumeration<TSelf, TKey> other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(other, null)) return false;

			return Id.Equals(other.Id);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 37 + Id.GetHashCode();
				return hash;
			}
		}

		public int CompareTo(object other)
		{
			return CompareTo(other as Enumeration<TSelf, TKey>);
		}

		public int CompareTo(Enumeration<TSelf, TKey> other)
		{
			if (ReferenceEquals(other, null)) throw new ArgumentNullException(nameof(other));
			return Id.CompareTo(other.Id);
		}
	}
}