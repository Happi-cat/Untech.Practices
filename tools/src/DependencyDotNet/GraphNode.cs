using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace DependencyDotNet
{
	[DebuggerDisplay("{Name}")]
	public class GraphNode
	{
		public GraphNode(AssemblyName assemblyName, Assembly assembly = null, IReadOnlyList<GraphNode> references = null)
			: this(assemblyName.Name, assemblyName.Version, assembly?.GetName().Version, references)
		{
		}

		public GraphNode(string name, Version version,
			Version foundVersion = null,
			IReadOnlyList<GraphNode> references = null)
		{
			Name = name;
			Version = version;
			FoundVersion = foundVersion;
			References = references;
		}

		public string Name { get; private set; }

		public Version Version { get; private set; }

		public Version FoundVersion { get; private set; }

		public IReadOnlyList<GraphNode> References { get; private set; }
	}
}
