using System.IO;
using System.Text;
using System.Xml;

namespace DependencyDotNet.Visitors
{
	public class SaveVisitor : DependencyGraphVisitor
	{
		private readonly XmlTextWriter _writer;

		public SaveVisitor(XmlTextWriter xmlTextWriter)
		{
			_writer = xmlTextWriter;
		}

		public override DependencyGraphNode Visit(DependencyGraphNode node)
		{
			if (node == null)
				return null;

			_writer.WriteStartElement("assembly");

			_writer.WriteAttributeString("name", node.Name);
			_writer.WriteAttributeString("version", node.Version.ToString());

			WriteIssues(node);

			Visit(node.References);

			_writer.WriteEndElement();

			return node;
		}


		private void WriteIssues(DependencyGraphNode node)
		{
			if (node.FoundVersion == null)
				WriteIssue("not-found", "");

			if (IsVersionMismatch(1))
				WriteIssue("major-version-mismatch", $"Version that was found: {node.FoundVersion}");
			else if (IsVersionMismatch(4))
				WriteIssue("version-mismatch", $"Version that was found: {node.FoundVersion}");

			bool IsVersionMismatch(int fieldCount)
			{
				return node.FoundVersion != null
					&& node.Version.ToString(fieldCount) != node.FoundVersion.ToString(fieldCount);
			}
		}

		private void WriteIssue(string type, string text)
		{
			_writer.WriteStartElement("issue");
			_writer.WriteAttributeString("type", type);
			_writer.WriteAttributeString("text", text);
			_writer.WriteEndElement();
		}
	}
}