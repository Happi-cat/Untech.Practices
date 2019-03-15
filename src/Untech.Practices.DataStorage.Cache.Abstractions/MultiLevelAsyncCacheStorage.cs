using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
		public async Task<CacheValue<T>> GetAsync<T>(string key,
			CancellationToken cancellationToken = default)
		{
			foreach (ICacheStorage cacheStorage in _cacheStorages)
			{
				CacheValue<T> value = await cacheStorage.GetAsync<T>(key, cancellationToken);

				if (value.HasValue) return value;
			}

			return default;
		}

		/// <inheritdoc />
		public Task SetAsync(string key, object value, CancellationToken cancellationToken = default)
		{
			return Task.WhenAll(_cacheStorages
				.Select(cacheStorage => cacheStorage.SetAsync(key, value, cancellationToken))
			);
		}

		/// <inheritdoc />
		public Task DropAsync(string key, CancellationToken cancellationToken = default)
		{
			return Task.WhenAll(_cacheStorages
				.Select(cacheStorage => cacheStorage.DropAsync(key, cancellationToken))
			);
		}
	}
}