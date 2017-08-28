using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices.Linq
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
		{
			return (source == null)
				? Enumerable.Empty<T>()
				: source;
		}
	

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