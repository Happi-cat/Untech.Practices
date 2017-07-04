using System.Collections;
using System.Collections.Generic;

namespace Untech.Practices.Collections
{
	public class RoundRobinCollection<T> : IEnumerable<T>
	{
		private readonly object _syncRoot = new object();

		private Node _tail;

		public void Push(T item)
		{
			lock (_syncRoot)
			{
				if (_tail == null)
				{
					_tail = new Node(item);
					_tail.Next = _tail;
				}
				else
				{
					_tail.Next = new Node(item)
					{
						Next = _tail.Next
					};
					// Shift tail for FIFO
					_tail = _tail.Next;
				}
			}
		}

		public T Pop()
		{
			lock (_syncRoot)
			{
				if (_tail == null)
				{
					return default(T);
				}
				else if (_tail.Next == _tail)
				{
					var oldNode = _tail;
					_tail = null;
					return oldNode.Value;
				}
				else
				{
					var oldHead = _tail.Next;
					_tail.Next = oldHead.Next;
					return oldHead.Value;
				}
			}
		}

		public T Peek()
		{
			var newHead = MoveNext();
			return newHead == null
				? default(T)
				: newHead.Value;
		}

		private Node MoveNext()
		{
			lock (_syncRoot)
			{
				return (_tail == null)
					? null
					: _tail = _tail.Next;
			}
		}

		public IEnumerator<T> GetEnumerator() => new Enumerator(this);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		private class Node
		{
			public Node(T value)
			{
				Value = value;
			}

			public T Value { get; set; }
			public Node Next { get; set; }
		}

		private class Enumerator : IEnumerator<T>
		{
			private readonly RoundRobinCollection<T> _parent;
			private Node _current;

			public Enumerator(RoundRobinCollection<T> parent)
			{
				_parent = parent;
			}

			public T Current => _current != null
				? _current.Value
				: default(T);

			object IEnumerator.Current => Current;

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				_current = _parent.MoveNext();
				return true;
			}

			public void Reset()
			{
			}
		}
	}
}
