using System;

namespace Untech.Practices.Monads
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct Maybe<T> : IEquatable<Maybe<T>>
	{
		/// <summary>
		/// The blank item.
		/// </summary>
		public static readonly Maybe<T> Nothing = new Maybe<T>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Maybe{T}"/> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public Maybe(T value)
		{
			HasValue = true;
			Value = value;
		}

		/// <summary>
		/// Gets a value indicating whether this instance has value.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
		/// </value>
		public bool HasValue { get; }
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public T Value { get; }

		/// <inheritdoc />
		public bool Equals(Maybe<T> other)
		{
			return HasValue == other.HasValue && Equals(Value, other.Value);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return HasValue ? "<" + Value + ">" : "<Nothing>";
		}
	}
}