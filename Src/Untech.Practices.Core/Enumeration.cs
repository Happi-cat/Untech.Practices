using System;
using System.Runtime.Serialization;

namespace Untech.Practices
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TSelf">The type of the self.</typeparam>
	/// <seealso cref="System.IComparable{Untech.Practices.Enumeration{TSelf}}" />
	/// <seealso cref="System.IEquatable{Untech.Practices.Enumeration{TSelf}}" />
	[DataContract]
	public abstract class Enumeration<TSelf> : IComparable<Enumeration<TSelf>>, IEquatable<Enumeration<TSelf>>
		where TSelf : Enumeration<TSelf>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Enumeration{TSelf}"/> class.
		/// </summary>
		protected Enumeration()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Enumeration{TSelf}"/> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="name">The name.</param>
		protected Enumeration(int id, string name)
		{
			Id = id;
			Name = name;
		}

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[DataMember]
		public int Id { get; private set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[DataMember]
		public string Name { get; private set; }

		public static bool operator ==(Enumeration<TSelf> left, Enumeration<TSelf> right)
		{
			return ReferenceEquals(left, null)
				? ReferenceEquals(right, null)
				: left.Equals(right);
		}

		public static bool operator !=(Enumeration<TSelf> left, Enumeration<TSelf> right)
		{
			return !(left == right);
		}

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

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TSelf">The type of the self.</typeparam>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <seealso cref="System.IComparable{Untech.Practices.Enumeration{TSelf}}" />
	/// <seealso cref="System.IEquatable{Untech.Practices.Enumeration{TSelf}}" />
	[DataContract]
	public abstract class Enumeration<TSelf, TKey> : IComparable<Enumeration<TSelf, TKey>>, IEquatable<Enumeration<TSelf, TKey>>
		where TSelf : Enumeration<TSelf, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Enumeration{TSelf, TKey}"/> class.
		/// </summary>
		protected Enumeration()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Enumeration{TSelf, TKey}"/> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="name">The name.</param>
		protected Enumeration(TKey id, string name)
		{
			Id = id;
			Name = name;
		}

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[DataMember]
		public TKey Id { get; private set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[DataMember]
		public string Name { get; private set; }

		public static bool operator ==(Enumeration<TSelf, TKey> left, Enumeration<TSelf, TKey> right)
		{
			return ReferenceEquals(left, null)
				? ReferenceEquals(right, null)
				: left.Equals(right);
		}

		public static bool operator !=(Enumeration<TSelf, TKey> left, Enumeration<TSelf, TKey> right)
		{
			return !(left == right);
		}

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