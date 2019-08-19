using System.Collections.Generic;
using CommandLine;

namespace DependencyDotNet
{
	public class Options
	{
		[Option('f', "file", Required = true, HelpText = "File name to analyze.")]
		public string File { get; set; }

		[Option("folders", Required = false, HelpText = "Folders to use for assembly search.")]
		public IEnumerable<string> Directories { get; set; }

		[Option("collapse", Required = false, HelpText = "Do not load dependencies for these libraries.")]
		public IEnumerable<string> CollapseAssemblies { get; set; }

		[Option("expand", Required = false, HelpText = "Forces to load dependencies for these libraries.")]
		public IEnumerable<string> ExpandAssemblies { get; set; }

		[Option("filter", Required = false)]
		public IEnumerable<string> FilterAssemblies { get; set; }

		[Option("find-path-to", Required = false)]
		public IEnumerable<string> FindPathTo { get; set; }


		[Option('o', "output", Required = false)]
		public string OutputeFile { get; set; }
	}
}
