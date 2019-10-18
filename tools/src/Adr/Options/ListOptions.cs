using CommandLine;

namespace Adr.Options
{
	[Verb("list", HelpText = "Display existing decisions")]
	public class ListOptions
	{
		[Option(Default = "docs/adr/")]
		public string Directory { get; set; }
	}
}