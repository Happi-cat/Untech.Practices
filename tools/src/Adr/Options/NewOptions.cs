using CommandLine;

namespace Adr.Options
{
	[Verb("new", HelpText = "Create Architecture Decision Records")]
	public class NewOptions
	{
		[Option('t', Required = true)]
		public string Title { get; set; }

		[Option('s', "supersedes", HelpText = "Use it to create a new ADR that supersedes a previous one")]
		public int? SupersedesAdrNo { get; set; }

		[Option(Default = "docs/adr/")]
		public string Directory { get; set; }
	}
}