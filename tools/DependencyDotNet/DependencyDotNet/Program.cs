using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace DependencyDotNet
{
	public class Options
	{
		[Option('f', "file", Required = true, HelpText = "File name to analyze.")]
		public string File { get; set; }

		[Option("folders", Required = false, HelpText = "Folders to use for assembly search.")]
		public IEnumerable<string> Directories { get; set; }

		[Option("skip", Required = false)]
		public IEnumerable<string> SkipAssemblies { get; set; }

		[Option('o', "output", Required = false)]
		public string OutputeFile { get; set; }
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			CommandLine.Parser.Default.ParseArguments<Options>(args)
	.WithParsed(opts =>
	{
		var xml = new DependencyGraphBuilder
		{
			Directories = opts.Directories.ToList(),
			NotExpandableAssemblies = opts.SkipAssemblies.ToList()
		}
		.Build(opts.File)
		.ToXml();

		if (opts.OutputeFile != null) xml.Save(opts.OutputeFile);
		else xml.Save(Console.Out);
	})
;
		}
	}
}
