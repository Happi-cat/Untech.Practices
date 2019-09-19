using System;
using System.IO;
using Xunit;

namespace Adr
{
	public class AdrEntryWriterTest
	{
		[Fact]
		public void WriteInitial_OK()
		{
			var entry = AdrEntry.CreateInitial();
			entry.When = new DateTime(2019, 9, 19);

			var actual = new StringWriter();
			new AdrEntryWriter(actual).Write(entry);

			Assert.Equal(Examples.InitialRecordMarkdown, actual.ToString());
		}

		[Fact]
		public void WriteNewLinesAndOmittedSections_OK()
		{
			var entry = Examples.CreateNewLinesAndOmittedSections();

			// act
			var actual = new StringWriter();
			new AdrEntryWriter(actual).Write(entry);

			// assert
			Assert.Equal(Examples.NewLinesAndOmittedSectionsMarkdown, actual.ToString());
		}
	}
}