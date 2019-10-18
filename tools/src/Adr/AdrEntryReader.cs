using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Adr
{
	public class AdrEntryReader
	{
		private readonly TextReader _reader;

		public AdrEntryReader(Stream stream) : this(new StreamReader(stream, Encoding.UTF8))
		{
		}

		public AdrEntryReader(TextReader reader)
		{
			_reader = reader;
		}

		public AdrEntry Read()
		{
			var firstLine = _reader.ReadLine();
			if (firstLine == null) return null;
			if (!firstLine.StartsWith("# ")) throw new InvalidDataException("Invalid file");

			var dotIndex  = firstLine.IndexOf('.');
			var number = int.Parse(firstLine.AsSpan(2, dotIndex - 2));
			var title = firstLine.Substring(dotIndex + 1).TrimStart();

			var entry = new AdrEntry(title)
			{
				Number = number
			};
			var sections = ReadSections().ToDictionary(n => n.Item1, n => n.Item2);
			var meta = sections.GetValueOrDefault("")
				.Split('\n', StringSplitOptions.RemoveEmptyEntries)
				.Where(l => !string.IsNullOrEmpty(l))
				.Select(l => l.Split(": "))
				.ToDictionary(n => n[0], n => n.ElementAtOrDefault(1));

			entry.When = DateTime.TryParse(meta.GetValueOrDefault("Date"), out var when) ? when : DateTime.Today;
			entry.Status = sections.GetValueOrDefault("## Status");
			entry.Context = sections.GetValueOrDefault("## Context");
			entry.Decision = sections.GetValueOrDefault("## Decision");
			entry.Consequences = sections.GetValueOrDefault("## Consequences");

			return entry;
		}

		private IEnumerable<(string, string)> ReadSections()
		{
			var sections = new Dictionary<string, string>();

			string currentSection = "";

			foreach (var line in ReadLines())
			{
				if (line.StartsWith("## "))
				{
					currentSection = line;
				}
				else
				{
					var section = sections.GetValueOrDefault(currentSection);
					sections[currentSection] = section +  Environment.NewLine + line;
				}
			}

			return sections.Select(p => (p.Key, p.Value.TrimNewLines()));
		}

		private IEnumerable<string> ReadLines()
		{
			string currentLine;

			while ((currentLine = _reader.ReadLine()) != null) yield return currentLine;
		}
	}
}