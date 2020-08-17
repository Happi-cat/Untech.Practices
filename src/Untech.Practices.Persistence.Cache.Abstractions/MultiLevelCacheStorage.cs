using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Persistence.Cache
{
	/// <summary>
	///     Provides ability to use multi-level <see cref="ICacheStorage" />.
	/// </summary>
	public class MultiLevelCacheStorage : ICacheStorage
	{
		private readonly IReadOnlyCollection<ICacheStorage> _caches;

		/// <summary>
		///     Initializes a new instance with a list of caches.
		/// </summary>
		/// <param name="caches">The list of caches starting from highest priority to lowest.</param>
		public MultiLevelCacheStorage(IEnumerable<ICacheStorage> caches)
		{
			_caches = new List<ICacheStorage>(caches);
		}

		/// <inheritdoc />
		public async Task<CacheValue<T>> GetAsync<T>(string key,
			CancellationToken cancellationToken = default)
		{
			foreach (ICacheStorage cacheStorage in _caches)
			{
				CacheValue<T> value = await cacheStorage.GetAsync<T>(key, cancellationToken);

				if (value.HasValue)
					return value;
			}

			return default;
		}

		/// <inheritdoc />
		public Task SetAsync(string key, object value, CancellationToken cancellationToken = default)
		{
			return Task.WhenAll(_caches
				.Select(cacheStorage => cacheStorage.SetAsync(key, value, cancellationToken))
			);
		}

		/// <inheritdoc />
		public Task DropAsync(string key, CancellationToken cancellationToken = default)
		{
			return Task.WhenAll(_caches
				.Select(cacheStorage => cacheStorage.DropAsync(key, cancellationToken))
			);
		}
	}
}
