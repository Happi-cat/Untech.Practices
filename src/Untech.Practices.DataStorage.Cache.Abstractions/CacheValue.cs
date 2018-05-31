namespace Untech.Practices.DataStorage.Cache
{
	public struct CacheValue<T>
	{
		private readonly bool _hasValue;
		private readonly T _value;

		public CacheValue(T value)
		{
			_value = value;
			_hasValue = true;
		}

		public bool HasValue => _hasValue;

		public T Value => _value;

		public static implicit operator T(CacheValue<T> value)
		{
			return value._value;
		}
	}
}