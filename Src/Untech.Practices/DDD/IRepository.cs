namespace Untech.Practices.DDD
{
	public interface IRepository<in TKey, TAggregate>
		where TAggregate : IAggregateRoot<TKey>
	{
		TAggregate FindOne(TKey key);
		TAggregate Save(TAggregate obj);
		void Delete(TAggregate obj);
	}
}
