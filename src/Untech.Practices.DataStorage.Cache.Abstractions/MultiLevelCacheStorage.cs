using System.Collections.Generic;

namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	///     Provides ability to use multi-level <see cref="ICacheStorage" />.
	/// </summary>
	public class MultiLevelCacheStorage : ICacheStorage
	{
		private readonly IReadOnlyCollection<ICacheStorage> _cacheStorages;

		/// <summary>
		///     Initializes a new instance with a list of cache storages.
		/// </summary>
		/// <param name="cacheStorages">The list of cache storages starting from highest priority to lowest.</param>
		public MultiLevelCacheStorage(IEnumerable<ICacheStorage> cacheStorages)
		{
			_cacheStorages = new List<ICacheStorage>(cacheStorages);
		}

		/// <inheritdoc />
		public CacheValue<T> Get<T>(string key)
		{
			foreach (ICacheStorage cacheStorage in _cacheStorages)
			{
				CacheValue<T> value = cacheStorage.Get<T>(key);

				if (value.HasValue) return value;
			}

			return default;
		}

		/// <inheritdoc />
		public void Set(string key, object value)
		{
			foreach (ICacheStorage cacheStorage in _cacheStorages) cacheStorage.Set(key, value);
		}

		/// <inheritdoc />
		public void Drop(string key)
		{
			foreach (ICacheStorage cacheStorage in _cacheStorages) cacheStorage.Drop(key);
		}
	}
}