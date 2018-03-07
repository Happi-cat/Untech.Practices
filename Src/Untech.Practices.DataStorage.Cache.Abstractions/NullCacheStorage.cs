namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	/// Represents dummy cache storage.
	/// </summary>
	public class NullCacheStorage : ICacheStorage
	{
		/// <inheritdoc />
		public T Get<T>(CacheKey key)
		{
			return default(T);
		}

		/// <inheritdoc />
		public void Set(CacheKey key, object value)
		{
		}

		/// <inheritdoc />
		public void Drop(CacheKey key, bool prefix = false)
		{
		}
	}
}