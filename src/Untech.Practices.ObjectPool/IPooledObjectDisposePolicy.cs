namespace Untech.Practices.ObjectPool
{
	public interface IPooledObjectDisposePolicy<T> where T : class
	{
		void Dispose(T obj);
	}
}