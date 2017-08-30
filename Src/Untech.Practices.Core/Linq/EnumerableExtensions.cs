using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices.Linq
{
	/// <summary>
	/// 
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
			return (source == null)
				? Enumerable.Empty<T>()
				: source;
		}

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
	}
}