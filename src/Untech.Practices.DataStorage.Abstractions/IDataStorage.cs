namespace Untech.Practices.DataStorage
{
	/// <summary>
	/// Defines CRUD methods for entities that inherits <see cref="IAggregateRoot{TKey}"/>.
	/// </summary>
	/// <typeparam name="T">Type of the entity.</typeparam>
	/// <typeparam name="TKey">Type of the entity key.</typeparam>
	public interface IDataStorage<T, TKey>
		where T: IAggregateRoot<TKey>
	{
		/// <summary>
		/// Finds entity with a specified <paramref name="key"/> in the store.
		/// </summary>
		/// <param name="key">Key of the entity to find.</param>
		/// <returns>Entity that was found; otherwise throws <see cref="AggregateRootNotFoundException"/>.</returns>
		/// <exception cref="AggregateRootNotFoundException">Entity with <paramref name="key"/> cannot be found.</exception>
		T Find(TKey key);

		/// <summary>
		/// Creates <paramref name="entity"/> in the store.
		/// </summary>
		/// <param name="entity">Entity to create.</param>
		/// <returns>Entity that was created in the store.</returns>
		T Create(T entity);

		/// <summary>
		/// Updates <paramref name="entity"/> in the store.
		/// </summary>
		/// <param name="entity">Entity to update.</param>
		/// <returns>Entity that was updated in the store.</returns>
		/// <exception cref="AggregateRootNotFoundException">Entity with <paramref name="key"/> cannot be found.</exception>
		T Update(T entity);

		/// <summary>
		/// Deletes <paramref name="entity"/> in the store.
		/// </summary>
		/// <param name="entity">Entity to delete.</param>
		/// <returns>True when deleted successfully; otherwise false.</returns>
		/// <exception cref="AggregateRootNotFoundException">Entity with <paramref name="key"/> cannot be found.</exception>
		bool Delete(T entity);
	}

	/// <summary>
	/// Defines CRUD methods for entities with <see cref="int"/> key that inherits <see cref="IAggregateRoot"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IDataStorage<T> : IDataStorage<T, int>
		where T: IAggregateRoot
	{

	}
}