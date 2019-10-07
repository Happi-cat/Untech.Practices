namespace Untech.Practices.DataStorage.Linq2Db
{
	public interface IEntityMapper<TEntity, TData, TKey>
		where TEntity : IHasKey<TKey>
		where TData : IHasKey<TKey>
	{
		TEntity Map(TData value);

		TData Map(TEntity value);
	}
}