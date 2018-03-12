using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	/// Defines common async methods for object caching.
	/// </summary>
	public interface IAsyncCacheStorage
	{
		/// <summary>
		/// Reads cached object asynchronously or returns default value.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		/// <typeparam name="T">Type of object to read and cast to.</typeparam>
		/// <returns>Cached object or default value.</returns>
		Task<T> GetAsync<T>(CacheKey key, CancellationToken cancellationToken = default (CancellationToken));

		/// <summary>
		/// Caches object asynchronously with a specified <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <param name="value">Object to cache.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		Task SetAsync(CacheKey key, object value, CancellationToken cancellationToken = default (CancellationToken));

		/// <summary>
		/// Invalidates cache object asynchronously with defined <paramref name="key"/> or prefix if it's present in cache.
		/// </summary>
		/// <param name="key">Cache key or prefix.</param>
		/// <param name="prefix">Indicates that <paramref name="key"/> is a prefix. Default value if false.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		Task DropAsync(CacheKey key, bool prefix = false, CancellationToken cancellationToken = default (CancellationToken));
	}
}