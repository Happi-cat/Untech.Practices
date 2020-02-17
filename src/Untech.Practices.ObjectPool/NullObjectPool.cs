using Microsoft.Extensions.ObjectPool;

namespace Untech.Practices.ObjectPool
{
	public sealed class NullObjectPool<T> : ObjectPool<T> where T : class
	{
		private readonly IPooledObjectPolicy<T> _policy;
		private readonly IPooledObjectDisposePolicy<T> _disposePolicy;

		public NullObjectPool(IPooledObjectPolicy<T> policy, IPooledObjectDisposePolicy<T> disposePolicy)
		{
			_policy = policy;
			_disposePolicy = disposePolicy;
		}

		public override T Get()
		{
			return _policy.Create();
		}

		public override void Return(T obj)
		{
			_disposePolicy.Dispose(obj);
		}
	}
}