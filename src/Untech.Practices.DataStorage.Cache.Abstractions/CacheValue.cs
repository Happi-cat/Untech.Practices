namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	/// Wraps cached object. Used for better non-nullable structs handling.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct CacheValue<T>
	{
		private readonly bool _hasValue;
		private readonly T _value;

		/// <summary>
		/// Initializes a new instance with the specified value that was retrieved from cache.
		/// </summary>
		/// <param name="value">The value from cache.</param>
		public CacheValue(T value)
		{
			_value = value;
			_hasValue = true;
		}

		/// <summary>
		/// Determines whether value was set in cache.
		/// </summary>
		public bool HasValue => _hasValue;

		/// <summary>
		/// Gets cached value or default.
		/// </summary>
		public T Value => _value;

		public static implicit operator T(CacheValue<T> value)
		{
			return value._value;
		}
	}
}