using System.Collections.Generic;
using System.Linq;

namespace DependencyDotNet.Visitors
{
	public class FindPathToVisitor : DependencyGraphVisitor
	{
		private readonly IEnumerable<string> _filters;

		public FindPathToVisitor(IEnumerable<string> filters)
		{
			_filters = filters;
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

			if (_filters.Any(maskOrName => Wildcard.IsMatch(node.Name, maskOrName)))
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