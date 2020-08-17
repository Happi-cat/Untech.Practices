using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Adr
{
	public class AdrDirectory
	{
		private readonly string _directory;

		public AdrDirectory(string directory)
		{
			_directory = directory;
		}

		public string GetAdrFilePath(int fileNumber, string title)
		{
			return Path.Combine(_directory, $"{fileNumber:0000}-{SanitizeFileName(title)}.md");
		}

		public int GetNextAdrNo()
		{
			if (!Directory.Exists(_directory)) return 1;

			var lastAdrNo = Directory.GetFiles(_directory, "*.md", SearchOption.TopDirectoryOnly)
				.Select(ParseAdrNo)
				.OrderByDescending(n => n)
				.FirstOrDefault();
			return lastAdrNo + 1;
		}

		private string SanitizeFileName(string title)
		{
			return title
				.Replace(' ', '-')
				.ToLower();
		}

		public AdrFile GetRecord(int number)
		{
			return Directory.GetFiles(_directory, "*.md", SearchOption.TopDirectoryOnly)
				.Where(f => ParseAdrNo(f) == number)
				.Select(AdrFile.Read)
				.FirstOrDefault();
		}

		public IEnumerable<AdrFile> GetRecords()
		{
			return Directory.GetFiles(_directory, "*.md", SearchOption.TopDirectoryOnly)
				.Where(f => ParseAdrNo(f) > 0)
				.OrderBy(f => f)
				.Select(AdrFile.Read);
		}

		private static int ParseAdrNo(string filePath)
		{
			var name = Path.GetFileName(filePath);
			return int.TryParse(name.AsSpan(0, 4), out var value) ? value : 0;
		}
	}
}