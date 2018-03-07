﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices.Linq
{
	/// <summary>
	/// Defines extension methods for <see cref="IEnumerable{T}"/>.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Returns empty <see cref="IEnumerable{T}"/> if null is passed.
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

			foreach (var element in source)
			{
				if (!predicate(element))
				{
					yield return element;
				}
			}
		}

		public static IEnumerable<T> ExceptNulls<T>(this IEnumerable<T> source) => source.Except(n => n == null);

		/// <summary>
		/// Executes the specified <paramref name="action" /> over each element of the sequence.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="action">The action.</param>
		/// <exception cref="ArgumentNullException">
		/// source
		/// or
		/// action
		/// </exception>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			source = source ?? throw new ArgumentNullException(nameof(source));
			action = action ?? throw new ArgumentNullException(nameof(action));

			foreach (var item in source)
			{
				action(item);
			}
		}

		public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
		{
			source = source ?? throw new ArgumentNullException(nameof(source));
			action = action ?? throw new ArgumentNullException(nameof(action));

			foreach (var item in source)
			{
				action(item);
				yield return item;
			}
		}

		public static IEnumerable<IReadOnlyList<T>> ToChunks<T>(this IEnumerable<T> source, int chunkSize)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (chunkSize <= 0) throw new ArgumentOutOfRangeException(nameof(chunkSize), chunkSize, "Cannot be less than 1");

			var snapshot = new List<T>(source);

			if (snapshot.Count == 0)
			{
			}
			else if (snapshot.Count <= chunkSize)
			{
				yield return snapshot;
			}
			else
			{
				var chunkIndex = 0;
				do
				{
					var realChunkSize = Math.Min(snapshot.Count - chunkIndex, chunkSize);
					yield return snapshot.GetRange(chunkIndex, realChunkSize);
					chunkIndex += realChunkSize;
				} while (chunkIndex < snapshot.Count);
			}
		}
	}
}