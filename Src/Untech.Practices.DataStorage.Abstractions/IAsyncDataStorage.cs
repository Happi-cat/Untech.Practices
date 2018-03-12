using System.Threading.Tasks;

namespace Untech.Practices.DataStorage
{
	/// <summary>
	/// Defines async CRUD methods for entities that inherits <see cref="IAggregateRoot{TKey}"/>.
	/// </summary>
	/// <typeparam name="T">Type of the entity.</typeparam>
	/// <typeparam name="TKey">Type of the entity key.</typeparam>
	public interface IAsyncDataStorage<T, TKey>
		where T: IAggregateRoot<TKey>
	{
		/// <summary>
		/// Finds entity asynchronously with a specified <paramref name="key"/> in the store.
		/// </summary>
		/// <param name="key">Key of the entity to find.</param>
		/// <returns>Entity that was found; otherwise throws <see cref="AggregateRootNotFoundException"/>.</returns>
		/// <exception cref="AggregateRootNotFoundException">Entity with <paramref name="key"/> cannot be found.</exception>
		Task<T> FindAsync(TKey key);

		/// <summary>
		/// Creates <paramref name="entity"/> asynchronously in the store.
		/// </summary>
		/// <param name="entity">Entity to create.</param>
		/// <returns>Entity that was created in the store.</returns>
		Task<T> CreateAsync(T entity);

		/// <summary>
		/// Updates <paramref name="entity"/> asynchronously in the store.
		/// </summary>
		/// <param name="entity">Entity to create.</param>
		/// <returns>Entity that was created in the store.</returns>
		/// <exception cref="AggregateRootNotFoundException">Entity with <paramref name="key"/> cannot be found.</exception>
		Task<T> UpdateAsync(T entity);

		/// <summary>
		/// Deletes <paramref name="entity"/> asynchronously in the store.
		/// </summary>
		/// <param name="entity">Entity to delete.</param>
		/// <returns>True when deleted successfully; otherwise false.</returns>
		/// <exception cref="AggregateRootNotFoundException">Entity with <paramref name="key"/> cannot be found.</exception>
		Task<bool> DeleteAsync(T entity);
	}

	/// <summary>
	/// Defines async CRUD methods for entities with <see cref="int"/> key that inherits <see cref="IAggregateRoot"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IAsyncDataStorage<T> : IAsyncDataStorage<T, int>
		where T : IAggregateRoot
	{

	}
}