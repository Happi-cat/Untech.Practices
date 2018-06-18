using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	/// Represents dummy cache storage.
	/// </summary>
	public sealed class NullCacheStorage : ICacheStorage, IAsyncCacheStorage
	{
		/// <inheritdoc />
		public CacheValue<T> Get<T>(string key)
		{
			return default(CacheValue<T>);
		}

		/// <inheritdoc />
		public Task<CacheValue<T>> GetAsync<T>(string key, CancellationToken cancellationToken = default (CancellationToken))
		{
			return Task.FromResult(default(CacheValue<T>));
		}

		/// <inheritdoc />
		public void Set(string key, object value)
		{
		}

		/// <inheritdoc />
		public Task SetAsync(string key, object value, CancellationToken cancellationToken = default (CancellationToken))
		{
			return Task.FromResult(0);
		}

		/// <inheritdoc />
		public void Drop(string key)
		{
		}

		/// <inheritdoc />
		public Task DropAsync(string key, CancellationToken cancellationToken = default (CancellationToken))
		{
			return Task.FromResult(0);
		}
	}
}