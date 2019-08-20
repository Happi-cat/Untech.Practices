using System.Collections.Generic;
using System.Linq;

namespace DependencyDotNet.Visitors
{
	public class FilterVisitor : GraphVisitor
	{
		private readonly IReadOnlyList<string> _filters;

		public FilterVisitor(IEnumerable<string> filters)
		{
			_filters = filters?.ToList();
		}

		public override GraphNode Visit(GraphNode node)
		{
			if (node == null)
				return null;

			return Wildcard.IsMatchAnyMask(node.Name, _filters)
				? new GraphNode(node.Name, node.Version, node.FoundVersion, Visit(node.References))
				: null;
		}
	}
}