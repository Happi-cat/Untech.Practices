using System.IO;

namespace Adr
{
	public sealed class AdrFile
	{
		public AdrFile(string path, AdrEntry entry)
		{
			FilePath = path;
			Entry = entry;
		}

		public string FilePath { get; private set; }
		public AdrEntry Entry { get; private set; }

		public string AsLink()
		{
			return $"[{Entry.Number}. {Entry.Title}]({Path.GetFileName(FilePath)})";
		}

		public void Save()
		{
			using (var stream = File.OpenWrite(FilePath))
			{
				new AdrEntryWriter(stream).Write(Entry);
			}
		}

		public static AdrFile Read(string filePath)
		{
			using (var stream = File.OpenRead(filePath))
			{
				var entry = new AdrEntryReader(stream).Read();
				return new AdrFile(filePath, entry);
			}
		}

		public static AdrFile Save(AdrDirectory directory, AdrEntry entry)
		{
			entry.Number = directory.GetNextAdrNo();
			var filePath = directory.GetAdrFilePath(entry.Number, entry.Title);

			using (var stream = File.OpenWrite(filePath))
			{
				new AdrEntryWriter(stream).Write(entry);
			}

			return new AdrFile(filePath, entry);
		}
	}
}
