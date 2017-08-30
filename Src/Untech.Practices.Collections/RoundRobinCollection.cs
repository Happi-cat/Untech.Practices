using System.Collections;
using System.Collections.Generic;

namespace Untech.Practices.Collections
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
	public class RoundRobinCollection<T> : IEnumerable<T>
	{
		private readonly object _syncRoot = new object();

		private Node _tail;

		/// <summary>
		/// Inserts an object at the head.
		/// </summary>
		/// <param name="item">The item.</param>
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

		/// <summary>
		/// Removes and returns the head object.
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Returns the head object without removing it and moves head to next object.
		/// </summary>
		/// <returns></returns>
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

			public T Value { get; }
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
