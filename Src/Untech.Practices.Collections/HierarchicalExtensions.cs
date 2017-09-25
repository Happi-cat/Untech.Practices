using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices.Collections
{
	public static class HierarchicalExtensions
	{
		public static IEnumerable<T> Descendants<T>(this T node)
			where T : IHierarchical<T> => node.SelectMany(DescendantsAndSelf);

		public static IEnumerable<T> DescendantsAndSelf<T>(this T node)
			where T : IHierarchical<T>
		{
			yield return node;

			foreach (var descendant in node.Descendants())
			{
				yield return descendant;
			}
		}
	}
}