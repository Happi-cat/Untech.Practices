namespace Untech.Practices.DataStorage
{
	/// <summary>
	///     Used as a marker for entities that can define and used own <see cref="IDataStorage{T,TKey}" />.
	/// </summary>
	/// <typeparam name="TKey">Type of the entity key.</typeparam>
	public interface IHasKey<TKey>
	{
		/// <summary>
		///     Gets entity key.
		/// </summary>
		TKey Key { get; }
	}

	/// <summary>
	///     Used as a marker for entities with <see cref="int" /> key that can define and use own
	///     <see cref="IDataStorage{T,TKey}" />.
	/// </summary>
	public interface IHasKey : IHasKey<int>
	{
	}
}