using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace DependencyDotNet
{
	[DataContract]
	[DebuggerDisplay("{Name}")]
	public class DependencyGraphNode
	{
		public DependencyGraphNode(AssemblyName assemblyName)
		{
			Name = assemblyName.Name;
			Version = assemblyName.Version.ToString();
		}

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Version { get; set; }

		[DataMember]
		public bool Collapsed { get; set; }

		[DataMember]
		public bool NotFound { get; set; }

		[DataMember]
		public List<DependencyGraphNode> References { get; set; }

		private XElement _xElement;

		public XElement ToXml()
		{
			return _xElement ?? (_xElement = new XElement("assembly",
								new XAttribute("name", Name),
								new XAttribute("version", Version),
								Collapsed ? new XAttribute("collapsed", true) : null,
								NotFound ? new XAttribute("found", false) : null,
								References?.Select(n => n.ToXml())
							));
		}
	}
}
