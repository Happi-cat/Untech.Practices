using System;
using System.IO;
using System.Linq;

namespace Adr
{
	public class AdrEntry
	{
		public AdrEntry(string title)
		{
			Title = title;
		}

		public string Title { get; private set; }

		public string Status { get; set; }

		public string Context { get; set; }

		public string Decision { get; set; }

		public string Consequences { get; set; }

		public string Links { get; set; }

		public static AdrEntry CreateInitial()
		{
			return new AdrEntry("Record architecture decisions")
			{
				Status = "Accepted",
				Context = "We need to record the architectural decisions made on this project.",
				Decision = "We will use Architecture Decision Records, as [described by Michael Nygard](http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions).",
				Consequences = "See Michael Nygard's article, linked above. For a lightweight ADR toolset, see Nat Pryce's [adr-tools](https://github.com/npryce/adr-tools)."
			};
		}

		public static AdrEntry CreateNew(string title)
		{
			return new AdrEntry(title)
			{
				Status = "Proposed", Context = "{context}", Decision = "{decision}", Consequences = "{consequences}"
			};
		}

		public string Save(string folder)
		{
			var fileNumber = GetNextAdrNo(folder);
			var filePath = Path.Combine(folder, $"{fileNumber:0000}-{SanitizeFileName(Title)}.md");

			using (var writer = File.CreateText(filePath))
			{
				writer.WriteLine($"# {fileNumber}. {Title}");
				writer.WriteLine();
				writer.WriteLine(DateTime.Today.ToString("yyyy-MM-dd"));
				writer.WriteLine();
				writer.WriteLine("## Status");
				writer.WriteLine();
				writer.WriteLine(Status);
				writer.WriteLine();
				writer.WriteLine("## Context");
				writer.WriteLine();
				writer.WriteLine(Context);
				writer.WriteLine();
				writer.WriteLine("## Decision");
				writer.WriteLine();
				writer.WriteLine(Decision);
				writer.WriteLine();
				writer.WriteLine("## Consequences");
				writer.WriteLine();
				writer.WriteLine(Consequences);
			}

			return filePath;
		}

		private static int GetNextAdrNo(string folder)
		{
			if (!Directory.Exists(folder)) return 1;

			var lastAdrNo = Directory.GetFiles(folder, "*.md", SearchOption.TopDirectoryOnly)
				.Select(ParseAdrNo)
				.OrderByDescending(n => n)
				.FirstOrDefault();
			return lastAdrNo + 1;

			int ParseAdrNo(string filePath)
			{
				var name = Path.GetFileName(filePath);
				return int.TryParse(name.AsSpan(0, 4), out var value) ? value : 0;
			}
		}

		private static string SanitizeFileName(string title)
		{
			return title
				.Replace(' ', '-')
				.ToLower();
		}
	}
}