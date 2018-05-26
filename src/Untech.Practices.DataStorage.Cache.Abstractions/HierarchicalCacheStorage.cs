using System.Collections.Generic;

namespace Untech.Practices.DataStorage.Cache
{
	public class HierarchicalCacheStorage : ICacheStorage
	{
		private readonly IReadOnlyCollection<ICacheStorage> _cacheStorages;

		public HierarchicalCacheStorage(IEnumerable<ICacheStorage> cacheStorages)
		{
			_cacheStorages = new List<ICacheStorage>(cacheStorages);
		}

		public CacheValue<T> Get<T>(CacheKey key)
		{
			foreach (var cacheStorage in _cacheStorages)
			{
				var value = cacheStorage.Get<T>(key);

				if (value.HasValue) return value;
			}

			return default(CacheValue<T>);
		}

		public void Set(CacheKey key, object value)
		{
			foreach (var cacheStorage in _cacheStorages)
			{
				cacheStorage.Set(key, value);
			}
		}

		public void Drop(CacheKey key, bool prefix = false)
		{
			foreach (var cacheStorage in _cacheStorages)
			{
				cacheStorage.Drop(key, prefix);
			}
		}
	}
}