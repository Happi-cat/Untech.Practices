using System;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;

namespace Untech.Practices.ObjectPool
{
	public class WeakObjectPool<T> : ObjectPool<T>, IDisposable where T : class
	{
		private readonly ObjectPool<WeakReference<T>> _objects;

		public WeakObjectPool([NotNull] IPooledObjectPolicy<T> policy, int maximumRetained)
			: this(policy, new DefaultPooledObjectDisposePolicy<T>(), maximumRetained)
		{
		}

		public WeakObjectPool([NotNull] IPooledObjectPolicy<T> policy,
			[NotNull] IPooledObjectDisposePolicy<T> disposePolicy,
			int maximumRetained)
		{
			if (policy == null) throw new ArgumentNullException(nameof(policy));
			if (disposePolicy == null) throw new ArgumentNullException(nameof(disposePolicy));

			var wrappedPolicy = new PolicyAdapter(policy);
			var wrappedDisposePolicy = new DisposePolicyAdapter(disposePolicy);

			_objects = new TieredObjectPool<WeakReference<T>>(
				wrappedPolicy,
				maximumRetained,
				new NullObjectPool<WeakReference<T>>(wrappedPolicy, wrappedDisposePolicy)
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

		public void Dispose()
		{
			Utils.TryDispose(_objects);
		}

		private class PolicyAdapter : PooledObjectPolicy<WeakReference<T>>
		{
			private readonly IPooledObjectPolicy<T> _policy;

			public PolicyAdapter(IPooledObjectPolicy<T> policy)
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
		}

		private class DisposePolicyAdapter : IPooledObjectDisposePolicy<WeakReference<T>>
		{
			private readonly IPooledObjectDisposePolicy<T> _policy;

			public DisposePolicyAdapter(IPooledObjectDisposePolicy<T> policy)
			{
				_policy = policy;
			}

			public void Dispose(WeakReference<T> obj)
			{
				if (obj.TryGetTarget(out var value))
				{
					_policy.Dispose(value);
				}
			}
		}
	}
}