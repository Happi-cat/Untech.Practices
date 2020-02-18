using System;

namespace Untech.Practices.ObjectPool
{
	public class DefaultPooledObjectDisposePolicy<T> : IPooledObjectDisposePolicy<T>
		where T : class
	{
		private static readonly bool s_disposable;

		static DefaultPooledObjectDisposePolicy()
		{
			s_disposable = typeof(IDisposable).IsAssignableFrom(typeof(T));
		}

		public void Dispose(T obj)
		{
			if (s_disposable)
			{
				((IDisposable)obj)?.Dispose();
			}
		}
	}
}