using System.Collections.Generic;
using System.Linq;

namespace DependencyDotNet.Visitors
{
	public class FindPathToVisitor : GraphVisitor
	{
		private readonly IList<string> _filters;

		public FindPathToVisitor(IEnumerable<string> filters)
		{
			_filters = filters?.ToList();
		}

		public override GraphNode Visit(GraphNode node)
		{
			if (node == null)
				return null;

			var refs = Visit(node.References);

			if (refs != null && refs.Any())
				return new GraphNode(node.Name, node.Version, node.FoundVersion, refs);

			return Wildcard.IsMatchAnyMask(node.Name, _filters)
				? new GraphNode(node.Name, node.Version, node.FoundVersion, refs)
				: null;
		}
	}
}