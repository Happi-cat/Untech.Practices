using System;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace DependencyDotNet
{

	internal class Program
	{
		static void Main(string[] args)
		{
			CommandLine.Parser.Default.ParseArguments<Options>(args)
				.WithParsed(opts =>
				{
					var xml = new DependencyGraphBuilder
					{
						Folders = opts.Directories.ToList(),
						CollapseAssemblies = opts.CollapseAssemblies.ToList(),
						FilterAssemblies = opts.FilterAssemblies.ToList()
					}
					.Build(opts.File)
					.ToXml();

					if (opts.OutputeFile != null)
						xml.Save(opts.OutputeFile);
					else
						xml.Save(Console.Out);
				})
;
		}
	}
}
