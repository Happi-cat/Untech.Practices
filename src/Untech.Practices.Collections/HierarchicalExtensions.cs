using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices.Collections
{
	/// <summary>
	///     This class provides extensions methods for <see cref="IHierarchical{T}" /> types.
	/// </summary>
	public static class HierarchicalExtensions
	{
		public static IEnumerable<T> Elements<T>(this IEnumerable<T> source)
			where T : IHierarchical<T>
		{
			return source == null
				? throw new ArgumentNullException(nameof(source))
				: source.SelectMany(n => n.Elements());
		}

		/// <summary>
		///     Returns all descendant elements.
		/// </summary>
		/// <param name="node">Current node to get descendants from.</param>
		/// <typeparam name="T">Hierarchical type.</typeparam>
		/// <returns>Sequence that contains descendants of <paramref name="node" />.</returns>
		public static IEnumerable<T> Descendants<T>(this T node)
			where T : IHierarchical<T>
		{
			return node == null
				? throw new ArgumentNullException(nameof(node))
				: node.Elements().SelectMany(DescendantsAndSelf);
		}

		public static IEnumerable<T> Descendants<T>(this IEnumerable<T> source)
			where T : IHierarchical<T>
		{
			return source == null
				? throw new ArgumentNullException(nameof(source))
				: source.SelectMany(Descendants);
		}

		/// <summary>
		///     Returns all descendant elements and self.
		/// </summary>
		/// <param name="node">Current node to get descendants from.</param>
		/// <typeparam name="T">Hierarchical type.</typeparam>
		/// <returns>Sequence that contains descendants of the <paramref name="node" /> and the <paramref name="node" /> itself.</returns>
		public static IEnumerable<T> DescendantsAndSelf<T>(this T node)
			where T : IHierarchical<T>
		{
			return node == null
				? throw new ArgumentNullException(nameof(node))
				: GetEnumerable();

			IEnumerable<T> GetEnumerable()
			{
				yield return node;

				foreach (T descendant in node.Descendants())
					yield return descendant;
			}
		}

		public static IEnumerable<T> DescendantsAndSelf<T>(this IEnumerable<T> source)
			where T : IHierarchical<T>
		{
			return source == null
				? throw new ArgumentNullException(nameof(source))
				: source.SelectMany(DescendantsAndSelf);
		}
	}
}