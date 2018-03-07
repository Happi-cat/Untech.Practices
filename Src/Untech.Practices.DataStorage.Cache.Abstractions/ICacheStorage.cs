namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	/// Defines common methods for object caching.
	/// </summary>
	public interface ICacheStorage
	{
		/// <summary>
		/// Reads cached object or returns default value.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <typeparam name="T">Type of object to read and cast to.</typeparam>
		/// <returns>Cached object or default value.</returns>
		T Get<T>(CacheKey key);

		/// <summary>
		/// Caches object with a specified <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <param name="value">Object to cache.</param>
		void Set(CacheKey key, object value);

		/// <summary>
		/// Invalidates cache object with defined <paramref name="key"/> or prefix if it's present in cache.
		/// </summary>
		/// <param name="key">Cache key or prefix.</param>
		/// <param name="prefix">Indicates that <paramref name="key"/> is a prefix. Default value if false.</param>
		void Drop(CacheKey key, bool prefix = false);
	}
}