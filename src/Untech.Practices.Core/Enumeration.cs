using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.Practices
{
	/// <summary>
	/// </summary>
	/// <typeparam name="TSelf">The type of the self.</typeparam>
	/// <seealso cref="System.IComparable{Untech.Practices.Enumeration{TSelf}}" />
	/// <seealso cref="System.IEquatable{Untech.Practices.Enumeration{TSelf}}" />
	[DataContract]
	public abstract class Enumeration<TSelf> : IComparable<TSelf>, IEquatable<TSelf>, IComparable
		where TSelf : Enumeration<TSelf>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Enumeration{TSelf}" /> class.
		/// </summary>
		protected Enumeration()
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Enumeration{TSelf}" /> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="name">The name.</param>
		protected Enumeration(int id, string name)
		{
			Id = id;
			Name = name;
		}

		/// <summary>
		///     Gets the identifier.
		/// </summary>
		/// <value>
		///     The identifier.
		/// </value>
		[DataMember]
		public int Id { get; private set; }

		/// <summary>
		///     Gets the name.
		/// </summary>
		/// <value>
		///     The name.
		/// </value>
		[DataMember]
		public string Name { get; private set; }

		public int CompareTo(object obj)
		{
			if (ReferenceEquals(obj, null)) throw new ArgumentNullException(nameof(obj));
			if (obj is TSelf self) return CompareTo(self);

			throw new ArgumentException("Invalid type", nameof(obj));
		}

		public int CompareTo(TSelf other)
		{
			if (ReferenceEquals(other, null)) throw new ArgumentNullException(nameof(other));
			return Id.CompareTo(other.Id);
		}

		public bool Equals(TSelf other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(other, null)) return false;
			return Id.Equals(other.Id);
		}

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
			return $"({Id}, {Name})";
		}


		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			if (ReferenceEquals(obj, null)) return false;
			if (obj is TSelf self) return Equals(self);
			return false;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 37 + Id;
				return hash;
			}
		}
	}

	/// <summary>
	/// </summary>
	/// <typeparam name="TSelf">The type of the self.</typeparam>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <seealso cref="System.IComparable{Untech.Practices.Enumeration{TSelf}}" />
	/// <seealso cref="System.IEquatable{Untech.Practices.Enumeration{TSelf}}" />
	[DataContract]
	public abstract class Enumeration<TSelf, TKey> : IComparable<TSelf>, IEquatable<TSelf>, IComparable
		where TSelf : Enumeration<TSelf, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Enumeration{TSelf, TKey}" /> class.
		/// </summary>
		protected Enumeration()
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Enumeration{TSelf, TKey}" /> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="name">The name.</param>
		protected Enumeration(TKey id, string name)
		{
			Id = id;
			Name = name;
		}

		/// <summary>
		///     Gets the identifier.
		/// </summary>
		/// <value>
		///     The identifier.
		/// </value>
		[DataMember]
		public TKey Id { get; private set; }

		/// <summary>
		///     Gets the name.
		/// </summary>
		/// <value>
		///     The name.
		/// </value>
		[DataMember]
		public string Name { get; private set; }

		public int CompareTo(object obj)
		{
			if (ReferenceEquals(obj, null)) throw new ArgumentNullException(nameof(obj));
			if (obj is TSelf self) return CompareTo(self);

			throw new ArgumentException("Invalid type", nameof(obj));
		}

		public int CompareTo(TSelf other)
		{
			if (ReferenceEquals(other, null)) throw new ArgumentNullException(nameof(other));
			return Comparer<TKey>.Default.Compare(Id, other.Id);
		}

		public bool Equals(TSelf other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(other, null)) return false;
			return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
		}

		public static bool operator ==(Enumeration<TSelf, TKey> left, Enumeration<TSelf, TKey> right)
		{
			if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
			return left.Equals(right);
		}

		public static bool operator !=(Enumeration<TSelf, TKey> left, Enumeration<TSelf, TKey> right)
		{
			return !(left == right);
		}

		public override string ToString()
		{
			return $"({Id}, {Name})";
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			if (ReferenceEquals(obj, null)) return false;
			if (obj is TSelf self) return Equals(self);
			return false;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 37 + EqualityComparer<TKey>.Default.GetHashCode(Id);
				return hash;
			}
		}
	}
}