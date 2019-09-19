using System;
using System.IO;
using Xunit;

namespace Adr
{
	public class AdrEntryReaderTest
	{
		[Fact]
		public void Read_OK()
		{
			// given
			var expected = AdrEntry.CreateInitial();
			expected.When = new DateTime(2019, 9, 19);

			// act
			var entry = new AdrEntryReader(new StringReader(MarkdownExamples.InitialRecord)).Read();

			// assert
			Assert.Equal(expected.Title, entry.Title);
			Assert.Equal(expected.Number, entry.Number);
			Assert.Equal(expected.Status, entry.Status);
			Assert.Equal(expected.Context, entry.Context);
			Assert.Equal(expected.Decision, entry.Decision);
			Assert.Equal(expected.Consequences, entry.Consequences);
			Assert.Equal(expected.When, entry.When);
		}
	}
}