using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Adr
{
	public class AdrEntryWriter
	{
		private readonly TextWriter _writer;

		public AdrEntryWriter(Stream stream) : this(new StreamWriter(stream, Encoding.UTF8))
		{

		}

		public AdrEntryWriter(TextWriter writer)
		{
			_writer = writer ?? throw new ArgumentNullException(nameof(writer));
		}

		public void Write(AdrEntry entry)
		{
			if (entry == null) throw new ArgumentNullException(nameof(entry));

			WriteHeading(1, $"{entry.Number}. {entry.Title}");

			WriteMeta(new[] { ("Date", entry.When.ToString("yyyy-MM-dd")) });

			WriteSection("Status", entry.Status);
			WriteSection("Context", entry.Context);
			WriteSection("Decision", entry.Decision);
			WriteSection("Consequences", entry.Consequences);

			_writer.Flush();
		}

		private void WriteHeading(byte level, string title)
		{
			var headingString = string.Empty.PadLeft(level, '#');
			WriteParagraph($"{headingString} {title}");
		}

		private void WriteMeta(IEnumerable<(string, string)> meta)
		{
			foreach (var (key, value) in meta) WriteParagraph($"{key}: {value}");
		}

		private void WriteSection(string header, string paragraph)
		{
			WriteHeading(2, header);
			WriteParagraph(paragraph.Trim('\n'));
		}

		private void WriteParagraph(string paragraph)
		{
			if (string.IsNullOrEmpty(paragraph)) return;

			_writer.WriteLine(paragraph);
			_writer.WriteLine();
		}
	}
}