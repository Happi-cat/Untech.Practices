namespace Untech.Practices.DataStorage.Cache
{
	public interface ICacheStorage
	{
		T Get<T>(CacheKey key);

		void Set(CacheKey key, object value);

		void Drop(CacheKey key, bool prefix = false);
	}
}