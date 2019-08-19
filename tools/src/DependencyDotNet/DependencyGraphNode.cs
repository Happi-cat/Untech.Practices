using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DependencyDotNet
{
	[DebuggerDisplay("{Name}")]
	public class DependencyGraphNode
	{
		public DependencyGraphNode(AssemblyName assemblyName)
			: this(assemblyName.Name, assemblyName.Version)
		{
		}

		public DependencyGraphNode(string name, Version version)
		{
			Name = name;
			Version = version;
		}

		public string Name { get; private set; }

		public Version Version { get; private set; }

		public Version FoundVersion { get; set; }

		public List<DependencyGraphNode> References { get; set; }
	}
}
