using System;
using System.Diagnostics;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;

namespace Untech.Practices.ObjectPool
{
	public class TieredObjectPool<T> : ObjectPool<T>, IDisposable
		where T : class
	{
		private readonly ObjectPool<T> _next;

		private readonly IPooledObjectPolicy<T> _policy;
		private readonly IPooledObjectDisposePolicy<T> _disposePolicy;

		private readonly ObjectWrapper[] _items;
		private T _firstItem;

		private bool _isDisposed;

		public TieredObjectPool([NotNull] IPooledObjectPolicy<T> policy,
			int maximumRetained,
			[NotNull] ObjectPool<T> next)
			: this(policy, new DefaultPooledObjectDisposePolicy<T>(), maximumRetained, next)
		{
		}

		public TieredObjectPool([NotNull] IPooledObjectPolicy<T> policy,
			[NotNull] IPooledObjectDisposePolicy<T> disposePolicy,
			int maximumRetained,
			[NotNull] ObjectPool<T> next)
		{
			_policy = policy ?? throw new ArgumentNullException(nameof(policy));
			_disposePolicy = disposePolicy ?? throw new ArgumentNullException(nameof(disposePolicy));
			_items = new ObjectWrapper[maximumRetained - 1];
			_next = next ?? throw new ArgumentNullException(nameof(next));
		}

		public override T Get()
		{
			if (_isDisposed) throw new ObjectDisposedException(GetType().Name);

			var item = _firstItem;
			if (item != null && Interlocked.CompareExchange(ref _firstItem, null, item) == item)
			{
				return item;
			}

			var items = _items;
			for (var i = 0; i < items.Length; i++)
			{
				item = items[i].Element;
				if (item != null && Interlocked.CompareExchange(ref items[i].Element, null, item) == item)
				{
					return item;
				}
			}

			return _next.Get();
		}

		public override void Return(T obj)
		{
			if (_isDisposed || !_policy.Return(obj))
			{
				_disposePolicy.Dispose(obj);
				return;
			}

			if (_firstItem == null && Interlocked.CompareExchange(ref _firstItem, obj, null) == null)
			{
				return;
			}

			var items = _items;
			for (var i = 0; i < items.Length; i++)
			{
				if (Interlocked.CompareExchange(ref items[i].Element, obj, null) == null)
				{
					return;
				}
			}

			_next.Return(obj);
		}

		public void Dispose()
		{
			_isDisposed = true;

			_disposePolicy.Dispose(_firstItem);
			_firstItem = null;

			var items = _items;
			for (var i = 0; i < items.Length; i++)
			{
				_disposePolicy.Dispose(items[i].Element);
				items[i].Element = null;
			}

			Utils.TryDispose(_next);
		}

		[DebuggerDisplay("{Element}")]
		private struct ObjectWrapper
		{
			public T Element;
		}
	}
}