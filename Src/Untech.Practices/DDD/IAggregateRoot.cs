namespace Untech.Practices.DDD
{
	public interface IAggregateRoot
	{
		bool CanBeSaved { get; }
	}

	public interface IAggregateRoot<TKey> : IAggregateRoot
	{
		TKey Key { get; }
	}
}
