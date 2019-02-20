using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.DataStorage.Cache
{
	/// <summary>
	/// Defines common async methods for object caching.
	/// </summary>
	public interface ICacheStorage
	{
		/// <summary>
		/// Reads cached object asynchronously or returns default value.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		/// <typeparam name="T">Type of object to read and cast to.</typeparam>
		/// <returns>Cached object or default value.</returns>
		Task<CacheValue<T>> GetAsync<T>(string key, CancellationToken cancellationToken = default);

		/// <summary>
		/// Caches object asynchronously with a specified <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <param name="value">Object to cache.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		Task SetAsync(string key, object value, CancellationToken cancellationToken = default);

		/// <summary>
		/// Invalidates cache object asynchronously with defined <paramref name="key"/> or prefix if it's present in cache.
		/// </summary>
		/// <param name="key">Cache key or prefix.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		Task DropAsync(string key, CancellationToken cancellationToken = default);
	}
}