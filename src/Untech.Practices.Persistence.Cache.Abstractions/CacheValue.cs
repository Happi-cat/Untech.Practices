namespace Untech.Practices.Persistence.Cache
{
	/// <summary>
	///     Wraps cached object. Used for better non-nullable structs handling.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct CacheValue<T>
	{
		/// <summary>
		///     Initializes a new instance with the specified value that was retrieved from cache.
		/// </summary>
		/// <param name="value">The value from cache.</param>
		public CacheValue(T value)
		{
			Value = value;
			HasValue = true;
		}

		/// <summary>
		///     Determines whether value was set in cache.
		/// </summary>
		public bool HasValue { get; }

		/// <summary>
		///     Gets cached value or default.
		/// </summary>
		public T Value { get; }

		public static implicit operator T(CacheValue<T> value)
		{
			return value.Value;
		}
	}
}
