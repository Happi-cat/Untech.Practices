namespace Untech.Practices.DataStorage
{
	public interface IAggregateRoot<TKey>
	{
		TKey Key { get; }
	}

	public interface IAggregateRoot : IAggregateRoot<int>
	{

	}
}