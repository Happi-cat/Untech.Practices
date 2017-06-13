namespace Untech.Practices.DDD
{
	public interface IAggregateRoot<TKey>
	{
		TKey Key { get; }

		bool CanBeSaved { get; }
	}
}
