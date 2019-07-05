namespace Untech.Practices.DataStorage.Linq2Db
{
	public interface IEntityMapper<T, TDao, TKey>
		where T : IHasKey<TKey>
		where TDao: IHasKey<TKey>
	{
		T Map(TDao value);

		TDao Map(T value);
	}
}