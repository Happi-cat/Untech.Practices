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
		public Task<T> GetAsync<T>(CacheKey key)
		{
			return Task.FromResult(default(T));
		}

		/// <inheritdoc />
		public void Set(CacheKey key, object value)
		{
		}

		/// <inheritdoc />
		public async Task SetAsync(CacheKey key, object value)
		{
		}

		/// <inheritdoc />
		public void Drop(CacheKey key, bool prefix = false)
		{
		}

		/// <inheritdoc />
		public async Task DropAsync(CacheKey key, bool prefix = false)
		{
		}
	}
}