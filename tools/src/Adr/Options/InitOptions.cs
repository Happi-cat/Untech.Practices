using CommandLine;

namespace Adr.Options
{
	[Verb("init", HelpText = "Create an ADR directory in the root of your project with record that you're using ADRs.")]
	public class InitOptions
	{
		[Option(Default = "docs/adr/")]
		public string Directory { get; set; }
	}
}