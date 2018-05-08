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
		public T Get<T>(CacheKey key)
		{
			return default(T);
		}

		/// <inheritdoc />
		public Task<T> GetAsync<T>(CacheKey key, CancellationToken cancellationToken = default (CancellationToken))
		{
			return Task.FromResult(default(T));
		}

		/// <inheritdoc />
		public void Set(CacheKey key, object value)
		{
		}

		/// <inheritdoc />
		public Task SetAsync(CacheKey key, object value, CancellationToken cancellationToken = default (CancellationToken))
		{
			return Task.FromResult(0);
		}

		/// <inheritdoc />
		public void Drop(CacheKey key, bool prefix = false)
		{
		}

		/// <inheritdoc />
		public Task DropAsync(CacheKey key, bool prefix = false, CancellationToken cancellationToken = default (CancellationToken))
		{
			return Task.FromResult(0);
		}
	}
}