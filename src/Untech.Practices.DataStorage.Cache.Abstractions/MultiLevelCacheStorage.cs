using System.Collections.Generic;

namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	/// Provides ability to use multi-level <see cref="ICacheStorage"/>.
	/// </summary>
	public class MultiLevelCacheStorage : ICacheStorage
	{
		private readonly IReadOnlyCollection<ICacheStorage> _cacheStorages;

		/// <summary>
		/// Initializes a new instance with a list of cache storages.
		/// </summary>
		/// <param name="cacheStorages">The list of cache storages starting from highest priority to lowest.</param>
		public MultiLevelCacheStorage(IEnumerable<ICacheStorage> cacheStorages)
		{
			_cacheStorages = new List<ICacheStorage>(cacheStorages);
		}

		/// <inheritdoc />
		public CacheValue<T> Get<T>(CacheKey key)
		{
			foreach (var cacheStorage in _cacheStorages)
			{
				var value = cacheStorage.Get<T>(key);

				if (value.HasValue) return value;
			}

			return default(CacheValue<T>);
		}

		/// <inheritdoc />
		public void Set(CacheKey key, object value)
		{
			foreach (var cacheStorage in _cacheStorages)
			{
				cacheStorage.Set(key, value);
			}
		}

		/// <inheritdoc />
		public void Drop(CacheKey key, bool prefix = false)
		{
			foreach (var cacheStorage in _cacheStorages)
			{
				cacheStorage.Drop(key, prefix);
			}
		}
	}
}