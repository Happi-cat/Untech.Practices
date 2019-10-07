using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	///     Represents dummy cache storage.
	/// </summary>
	public sealed class NullCacheStorage : ICacheStorage
	{
		/// <inheritdoc />
		public Task<CacheValue<T>> GetAsync<T>(string key,
			CancellationToken cancellationToken = default)
		{
			return Task.FromResult(default(CacheValue<T>));
		}

		/// <inheritdoc />
		public Task SetAsync(string key, object value, CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task DropAsync(string key, CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}
	}
}