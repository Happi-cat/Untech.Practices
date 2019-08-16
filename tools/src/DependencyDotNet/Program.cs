using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
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
					var node = new DependencyGraphBuilder
					{
						Folders = opts.Directories.ToList(),
						CollapseAssemblies = opts.CollapseAssemblies.ToList(),
						FilterAssemblies = opts.FilterAssemblies.ToList()
					}
					.Build(opts.File);


					if (opts.OutputeFile != null)
					{
						using (var file = File.Open(opts.OutputeFile, FileMode.Create))
						{
							node.Save(file);
						}
					}
					else
					{
						node.Save(Console.Out);
					}
				})
;
		}
	}
}
