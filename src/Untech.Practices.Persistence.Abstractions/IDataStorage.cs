﻿using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Persistence
{
	/// <summary>
	///     Defines async CRUD methods for entities that inherit <see cref="IHasKey{TKey}" />.
	/// </summary>
	/// <typeparam name="T">Type of the entity.</typeparam>
	/// <typeparam name="TKey">Type of the entity key.</typeparam>
	public interface IDataStorage<T, TKey>
		where T : IHasKey<TKey>
	{
		/// <summary>
		///     Finds entity asynchronously with a specified <paramref name="key" /> in the store.
		/// </summary>
		/// <param name="key">Key of the entity to find.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		/// <returns>Entity that was found; otherwise throws <see cref="ItemNotFoundException" />.</returns>
		/// <exception cref="ItemNotFoundException">Entity with <paramref name="key" /> cannot be found.</exception>
		Task<T> GetAsync(TKey key, CancellationToken cancellationToken = default);

		/// <summary>
		///     Creates <paramref name="entity" /> asynchronously in the store.
		/// </summary>
		/// <param name="entity">Entity to create.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		/// <returns>Entity that was created in the store.</returns>
		Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

		/// <summary>
		///     Updates <paramref name="entity" /> asynchronously in the store.
		/// </summary>
		/// <param name="entity">Entity to update.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		/// <returns>Entity that was created in the store.</returns>
		/// <exception cref="ItemNotFoundException">Entity with <paramref name="key" /> cannot be found.</exception>
		Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

		/// <summary>
		///     Deletes <paramref name="entity" /> asynchronously in the store.
		/// </summary>
		/// <param name="entity">Entity to delete.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		/// <returns>True when deleted successfully; otherwise false.</returns>
		/// <exception cref="ItemNotFoundException">Entity with <paramref name="key" /> cannot be found.</exception>
		Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);
	}

	/// <summary>
	///     Defines async CRUD methods for entities with <see cref="int" /> key that inherits <see cref="IHasKey" />.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IDataStorage<T> : IDataStorage<T, int>
		where T : IHasKey
	{
	}
}
