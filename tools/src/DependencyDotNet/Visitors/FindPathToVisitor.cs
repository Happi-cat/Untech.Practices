using System.Collections.Generic;
using System.Linq;

namespace DependencyDotNet.Visitors
{
	public class FindPathToVisitor : DependencyGraphVisitor
	{
		private readonly IList<string> _filters;

		public FindPathToVisitor(IEnumerable<string> filters)
		{
			_filters = filters?.ToList();
		}

		public override DependencyGraphNode Visit(DependencyGraphNode node)
		{
			if (node == null)
				return null;

			var refs = Visit(node.References);

			if (refs != null && refs.Any())
			{
				return new DependencyGraphNode(node.Name, node.Version)
				{
					FoundVersion = node.FoundVersion,
					References = refs
				};
			}

			if (Wildcard.IsMatchAnyMask(node.Name, _filters))
			{
				return new DependencyGraphNode(node.Name, node.Version)
				{
					FoundVersion = node.FoundVersion,
					References = refs
				};
			}

			return null;
		}
	}
}