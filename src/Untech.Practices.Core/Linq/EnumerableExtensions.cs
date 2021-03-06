﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices.Linq
{
	/// <summary>
	///     Defines extension methods for <see cref="IEnumerable{T}" />.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		///     Returns empty <see cref="IEnumerable{T}" /> if null is passed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
		{
			return source ?? Enumerable.Empty<T>();
		}

		public static IEnumerable<T> Except<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			source = source ?? throw new ArgumentNullException(nameof(source));
			predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

			return GetEnumerable();

			IEnumerable<T> GetEnumerable()
			{
				foreach (T element in source)
					if (!predicate(element))
						yield return element;
			}
		}

		public static IEnumerable<T> ExceptNulls<T>(this IEnumerable<T> source)
		{
			return source.Except(n => n == null);
		}

		/// <summary>
		///     Executes the specified <paramref name="action" /> over each element of the sequence.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="action">The action.</param>
		/// <exception cref="ArgumentNullException">
		///     source
		///     or
		///     action
		/// </exception>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			source = source ?? throw new ArgumentNullException(nameof(source));
			action = action ?? throw new ArgumentNullException(nameof(action));

			foreach (T item in source)
				action(item);
		}

		public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
		{
			source = source ?? throw new ArgumentNullException(nameof(source));
			action = action ?? throw new ArgumentNullException(nameof(action));

			return GetEnumerable();

			IEnumerable<T> GetEnumerable()
			{
				foreach (T item in source)
				{
					action(item);
					yield return item;
				}
			}
		}

		public static IEnumerable<IReadOnlyList<T>> ToChunks<T>(this IEnumerable<T> source, int chunkSize)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (chunkSize <= 0)
				throw new ArgumentOutOfRangeException(nameof(chunkSize), chunkSize, "Cannot be less than 1");

			return Chunks();

			IEnumerable<List<T>> Chunks()
			{
				IEnumerator<T> enumerator = source.GetEnumerator();
				while (enumerator.MoveNext())
					// moved to first element and going to return chunk
					yield return new List<T>(Chunk(enumerator));
				// if last chunk was smaller then chunk size
				// then MoveNext would be called twice (in Chunk method and current loop)
			}

			IEnumerable<T> Chunk(IEnumerator<T> enumerator)
			{
				int itemsReturned = 0;
				do
				{
					// chunk iterator starts when already moved next
					yield return enumerator.Current;
					// if chunk completed then exit without moving next (short circuit)
				} while (++itemsReturned < chunkSize && enumerator.MoveNext());
			}
		}

		public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			return condition
				? source.Where(predicate)
				: source;
		}

		/// <summary>
		///     Orders elements by position of keys in <paramref name="orderedKeys" /> sequence.
		///     Elements whose keys are not in <paramref name="orderedKeys" /> sequence would be returned afterwards in ascending
		///     order.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="keySelector"></param>
		/// <param name="orderedKeys"></param>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <returns></returns>
		public static IOrderedEnumerable<T> OrderByPosition<T, TKey>(this IEnumerable<T> source,
			Func<T, TKey> keySelector,
			IEnumerable<TKey> orderedKeys)
		{
			PositionalComparer<TKey> comparer = new PositionalComparer<TKey>(orderedKeys.EmptyIfNull());

			return source.OrderBy(keySelector, comparer);
		}

		public static IOrderedEnumerable<T> OrderByPosition<T, TKey>(this IEnumerable<T> source,
			Func<T, TKey> keySelector,
			IEnumerable<TKey> orderedKeys,
			IComparer<TKey> alternateComparer)
		{
			PositionalComparer<TKey> comparer =
				new PositionalComparer<TKey>(orderedKeys.EmptyIfNull(), alternateComparer);

			return source.OrderBy(keySelector, comparer);
		}

		public static IOrderedEnumerable<T> ThenByPosition<T, TKey>(this IOrderedEnumerable<T> source,
			Func<T, TKey> keySelector,
			IEnumerable<TKey> orderedKeys)
		{
			PositionalComparer<TKey> comparer = new PositionalComparer<TKey>(orderedKeys.EmptyIfNull());

			return source.ThenBy(keySelector, comparer);
		}

		public static IOrderedEnumerable<T> ThenByPosition<T, TKey>(this IOrderedEnumerable<T> source,
			Func<T, TKey> keySelector,
			IEnumerable<TKey> orderedKeys,
			IComparer<TKey> alternateComparer)
		{
			PositionalComparer<TKey> comparer =
				new PositionalComparer<TKey>(orderedKeys.EmptyIfNull(), alternateComparer);

			return source.ThenBy(keySelector, comparer);
		}

		private class PositionalComparer<T> : IComparer<T>
		{
			private readonly IComparer<T> _alternateComparer;
			private readonly int _elementNotFoundIndex;
			private readonly IReadOnlyDictionary<T, int> _orderedElements;

			public PositionalComparer(IEnumerable<T> orderedElements, IComparer<T> alternateComparer = null)
			{
				_orderedElements = orderedElements
					.Select((n, i) => Tuple.Create(n, i))
					.ToDictionary(n => n.Item1, n => n.Item2);

				_elementNotFoundIndex = alternateComparer == null
					? _orderedElements.Count
					: -1; // -1 mean to use alternate comparer

				_alternateComparer = alternateComparer;
			}

			public int Compare(T x, T y)
			{
				int indexX = IndexOf(x);
				int indexY = IndexOf(y);

				// compare by indexes
				if (indexX > -1 && indexY > -1)
					return indexX.CompareTo(indexY);

				// ordered elements will be less than elements not in orderedElementsCollection
				if (indexX > -1)
					return -1; // means x < y
				if (indexY > -1)
					return 1; // means x > y

				// usual compare
				return _alternateComparer.Compare(x, y);
			}

			private int IndexOf(T element)
			{
				return _orderedElements.TryGetValue(element, out int index)
					? index
					: _elementNotFoundIndex;
			}
		}
	}
}