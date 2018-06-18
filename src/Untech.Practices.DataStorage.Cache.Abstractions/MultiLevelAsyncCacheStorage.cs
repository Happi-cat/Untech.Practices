using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	/// Provides ability to use multi-level <see cref="IAsyncCacheStorage"/>.
	/// </summary>
	public class MultiLevelAsyncCacheStorage : IAsyncCacheStorage
	{
		private readonly IReadOnlyCollection<IAsyncCacheStorage> _cacheStorages;

		/// <summary>
		/// Initializes a new instance with a list of cache storages.
		/// </summary>
		/// <param name="cacheStorages">The list of cache storages starting from highest priority to lowest.</param>
		public MultiLevelAsyncCacheStorage(IEnumerable<IAsyncCacheStorage> cacheStorages)
		{
			_cacheStorages = new List<IAsyncCacheStorage>(cacheStorages);
		}

		/// <inheritdoc />
		public async Task<CacheValue<T>> GetAsync<T>(string key, CancellationToken cancellationToken = default(CancellationToken))
		{
			foreach (var cacheStorage in _cacheStorages)
			{
				var value = await cacheStorage.GetAsync<T>(key, cancellationToken);

				if (value.HasValue) return value;
			}

			return default(CacheValue<T>);
		}

		/// <inheritdoc />
		public Task SetAsync(string key, object value, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.WhenAll(_cacheStorages
				.Select(cacheStorage => cacheStorage.SetAsync(key, value, cancellationToken))
			);
		}

		/// <inheritdoc />
		public Task DropAsync(string key, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.WhenAll(_cacheStorages
				.Select(cacheStorage => cacheStorage.DropAsync(key, cancellationToken))
			);
		}
	}
}