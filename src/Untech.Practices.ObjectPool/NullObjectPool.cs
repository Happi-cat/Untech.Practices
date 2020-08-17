using System;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;

namespace Untech.Practices.ObjectPool
{
	public sealed class NullObjectPool<T> : ObjectPool<T> where T : class
	{
		private readonly IPooledObjectPolicy<T> _policy;
		private readonly IPooledObjectDisposePolicy<T> _disposePolicy;

		public NullObjectPool([NotNull] IPooledObjectPolicy<T> policy)
			: this(policy, new DefaultPooledObjectDisposePolicy<T>())
		{
		}

		public NullObjectPool([NotNull] IPooledObjectPolicy<T> policy,
			[NotNull] IPooledObjectDisposePolicy<T> disposePolicy)
		{
			_policy = policy ?? throw new ArgumentNullException(nameof(policy));
			_disposePolicy = disposePolicy ?? throw new ArgumentNullException(nameof(disposePolicy));
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