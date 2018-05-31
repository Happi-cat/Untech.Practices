using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.DataStorage.Cache
{
	public class HierarchicalAsyncCacheStorage : IAsyncCacheStorage
	{
		private readonly IReadOnlyCollection<IAsyncCacheStorage> _cacheStorages;

		public HierarchicalAsyncCacheStorage(IEnumerable<IAsyncCacheStorage> cacheStorages)
		{
			_cacheStorages = new List<IAsyncCacheStorage>(cacheStorages);
		}

		public async Task<CacheValue<T>> GetAsync<T>(CacheKey key, CancellationToken cancellationToken = default(CancellationToken))
		{
			foreach (var cacheStorage in _cacheStorages)
			{
				var value = await cacheStorage.GetAsync<T>(key, cancellationToken);

				if (value.HasValue) return value;
			}

			return default(CacheValue<T>);
		}

		public Task SetAsync(CacheKey key, object value, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.WhenAll(_cacheStorages
				.Select(cacheStorage => cacheStorage.SetAsync(key, value, cancellationToken))
			);
		}

		public Task DropAsync(CacheKey key, bool prefix = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.WhenAll(_cacheStorages
				.Select(cacheStorage => cacheStorage.DropAsync(key, prefix, cancellationToken))
			);
		}
	}
}