using System;
using Microsoft.Extensions.ObjectPool;

namespace Untech.Practices.ObjectPool
{
	public class WeakObjectPool<T> : ObjectPool<T> where T : class
	{
		private readonly ObjectPool<WeakReference<T>> _objects;

		public WeakObjectPool(IPooledObjectPolicy<T> policy, int maximumRetained)
		{
			var wrappedPolicy = new Policy(policy);

			_objects = new TieredObjectPool<WeakReference<T>>(
				wrappedPolicy,
				maximumRetained,
				new NullObjectPool<WeakReference<T>>(wrappedPolicy, wrappedPolicy)
			);
		}

		public override T Get()
		{
			T target;
			WeakReference<T> weakRef;

			do { weakRef = _objects.Get(); } while (!weakRef.TryGetTarget(out target));

			return target;
		}

		public override void Return(T item)
		{
			_objects.Return(new WeakReference<T>(item));
		}

		private class Policy : PooledObjectPolicy<WeakReference<T>>, IPooledObjectDisposePolicy<WeakReference<T>>
		{
			private readonly IPooledObjectPolicy<T> _policy;

			public Policy(IPooledObjectPolicy<T> policy)
			{
				_policy = policy;
			}

			public override WeakReference<T> Create()
			{
				return new WeakReference<T>(_policy.Create());
			}

			public override bool Return(WeakReference<T> obj)
			{
				return !obj.TryGetTarget(out var value) || _policy.Return(value);
			}

			public void Dispose(WeakReference<T> obj)
			{
				if (obj.TryGetTarget(out var value) && value is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}
	}
}