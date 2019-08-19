using System.Collections.Generic;
using System.Linq;

namespace DependencyDotNet.Visitors
{
	public abstract class DependencyGraphVisitor
	{
		public abstract DependencyGraphNode Visit(DependencyGraphNode node);

		protected virtual List<DependencyGraphNode> Visit(List<DependencyGraphNode> node)
		{
			return node?.Select(Visit).Where(n => n != null).ToList();
		}
	}
}