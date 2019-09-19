using System;
using System.IO;
using Xunit;

namespace Adr
{
	public class AdrEntryReaderTest
	{
		[Fact]
		public void ReadInitial_OK()
		{
			var expected = AdrEntry.CreateInitial();
			expected.When = new DateTime(2019, 9, 19);

			var entry = new AdrEntryReader(new StringReader(Examples.InitialRecordMarkdown)).Read();

			AssertEntries(expected, entry);
		}

		[Fact]
		public void ReadNewLinesAndOmittedSections_OK()
		{
			var expected = Examples.CreateNewLinesAndOmittedSections();

			var actual = new AdrEntryReader(new StringReader(Examples.NewLinesAndOmittedSectionsMarkdown)).Read();

			AssertEntries(expected, actual);
		}

		private static void AssertEntries(AdrEntry expected, AdrEntry actual)
		{
			Assert.Equal(expected.Title, actual.Title);
			Assert.Equal(expected.Number, actual.Number);
			Assert.Equal(expected.Status, actual.Status);
			Assert.Equal(expected.Context, actual.Context);
			Assert.Equal(expected.Decision, actual.Decision);
			Assert.Equal(expected.Consequences, actual.Consequences);
			Assert.Equal(expected.When, actual.When);
		}
	}
}