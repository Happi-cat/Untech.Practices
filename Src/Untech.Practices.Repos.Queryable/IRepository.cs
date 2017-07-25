namespace Untech.Practices.Repos.Queryable
{
	public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
	{
		TEntity Create(TEntity entity);

		bool Update(TEntity entity);

		bool Delete(TEntity entity);
	}
}