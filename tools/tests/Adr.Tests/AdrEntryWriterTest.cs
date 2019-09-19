using System;
using System.IO;
using Xunit;

namespace Adr
{
	public class AdrEntryWriterTest
	{
		[Fact]
		public void Write_OK()
		{
			// given
			var entry = AdrEntry.CreateInitial();
			entry.When = new DateTime(2019, 9, 19);

			// act
			var result = new StringWriter();
			new AdrEntryWriter(result).Write(entry);

			// assert
			Assert.Equal(MarkdownExamples.InitialRecord, result.ToString());
		}
	}
}