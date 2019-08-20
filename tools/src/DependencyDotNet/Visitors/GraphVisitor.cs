using System.Collections.Generic;
using System.Linq;

namespace DependencyDotNet.Visitors
{
	public abstract class GraphVisitor
	{
		public abstract GraphNode Visit(GraphNode node);

		protected virtual IReadOnlyList<GraphNode> Visit(IEnumerable<GraphNode> node)
		{
			return node?.Select(Visit).Where(n => n != null).ToList();
		}
	}
}