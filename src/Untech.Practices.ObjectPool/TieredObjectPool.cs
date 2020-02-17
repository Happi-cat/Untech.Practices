using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.ObjectPool;

namespace Untech.Practices.ObjectPool
{
	public class TieredObjectPool<T> : ObjectPool<T> where T : class
	{
		private readonly ObjectPool<T> _next;

		private readonly IPooledObjectPolicy<T> _policy;
		private readonly ObjectWrapper[] _items;
		private T _firstItem;

		public TieredObjectPool(IPooledObjectPolicy<T> policy, int maximumRetained, ObjectPool<T> next)
		{
			_policy = policy;
			_items = new ObjectWrapper[maximumRetained - 1];
			_next = next ?? throw new ArgumentNullException(nameof(next));
		}

		public override T Get()
		{
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
			if (_policy != null && !_policy.Return(obj)) return;

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

		[DebuggerDisplay("{Element}")]
		private struct ObjectWrapper
		{
			public T Element;
		}
	}
}