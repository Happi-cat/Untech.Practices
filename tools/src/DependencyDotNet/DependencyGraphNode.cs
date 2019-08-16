using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
		{
			Name = assemblyName.Name;
			Version = assemblyName.Version;
		}

		public string Name { get; set; }

		public Version Version { get; set; }

		public Version FoundVersion { get; set; }

		public bool Collapsed { get; set; }

		public bool NotFound { get; set; }

		public List<DependencyGraphNode> References { get; set; }

		public void Save(Stream stream)
		{
			using (var writer = new XmlTextWriter(stream, Encoding.UTF8)
			{
				IndentChar = ' ',
				Indentation = 2,
				Formatting = Formatting.Indented
			})
			{
				Save(writer);
			}
		}
		
		public void Save(TextWriter writer)
		{
			using (var xmlWriter = new XmlTextWriter(writer)
			{
				IndentChar = ' ',
				Indentation = 2,
				Formatting = Formatting.Indented
			})
			{
				Save(xmlWriter);
			}
		}

		public void Save(XmlTextWriter writer)
		{
			writer.WriteStartElement("assembly");

			writer.WriteAttributeString("name", Name);
			writer.WriteAttributeString("version", Version.ToString());

			if (Collapsed)
				writer.WriteAttributeString("collapsed", "true");

			WriteIssues(writer);

			if (References != null)
			{
				foreach (var reference in References)
					reference.Save(writer);
			}

			writer.WriteEndElement();
		}

		private void WriteIssues(XmlTextWriter writer)
		{
			if (NotFound)
				WriteIssue(writer, "not-found", "");

			if (IsMajorVersionMismatch())
				WriteIssue(writer, "major-version-mismatch", $"Version that was found: {FoundVersion}");
			else if (IsVersionMismatch())
				WriteIssue(writer, "version-mismatch", $"Version that was found: {FoundVersion}");
		}

		private void WriteIssue(XmlTextWriter writer, string type, string text)
		{
			writer.WriteStartElement("issue");
			writer.WriteAttributeString("type", type);
			writer.WriteAttributeString("text", text);
			writer.WriteEndElement();
		}

		public bool IsMajorVersionMismatch()
		{
			return FoundVersion != null && Version.ToString(1) != FoundVersion.ToString(1);
		}

		public bool IsVersionMismatch()
		{
			return FoundVersion != null && Version != FoundVersion;
		}
	}
}
