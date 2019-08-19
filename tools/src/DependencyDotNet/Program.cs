using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using CommandLine;
using CommandLine.Text;
using DependencyDotNet.Visitors;

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

					if (opts.FindPathTo != null && opts.FindPathTo.Any())
					{
						node = new FindPathToVisitor(opts.FindPathTo).Visit(node);
					}

					if (opts.OutputeFile != null)
						Save(opts.OutputeFile, node);
					else
						Save(Console.Out, node);
				});
		}

		private static void Save(string fileName, DependencyGraphNode node)
		{
			using (var stream = File.Open(fileName, FileMode.Create))
			using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8)
			{
				IndentChar = ' ', Indentation = 2, Formatting = Formatting.Indented
			})
			{
				new SaveVisitor(xmlWriter).Visit(node);
			}
		}

		private static void Save(TextWriter writer, DependencyGraphNode node)
		{
			using (var xmlWriter = new XmlTextWriter(writer)
			{
				IndentChar = ' ', Indentation = 2, Formatting = Formatting.Indented
			})
			{
				new SaveVisitor(xmlWriter).Visit(node);
			}
		}
	}
}
