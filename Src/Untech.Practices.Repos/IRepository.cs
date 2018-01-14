namespace Untech.Practices.Repos
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	/// <seealso cref="Untech.Practices.Repos.IReadOnlyRepository{TEntity}" />
	public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
	{
		/// <summary>
		/// Creates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		TEntity Create(TEntity entity);

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		bool Update(TEntity entity);

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		bool Delete(TEntity entity);
	}
}