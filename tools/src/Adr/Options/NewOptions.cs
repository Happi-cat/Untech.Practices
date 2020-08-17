using CommandLine;

namespace Adr.Options
{
	[Verb("new", HelpText = "Create Architecture Decision Records")]
	public class NewOptions
	{
		[Option('t', Required = true)]
		public string Title { get; set; }

		[Option('s', "supersedes")]
		public int? SupersedesAdr { get; set; }

		[Option('a', "amends")]
		public int? AmendsAdr { get; set; }

		[Option(Default = "docs/adr/")]
		public string Directory { get; set; }
	}
}