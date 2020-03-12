using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using CommandLine;
using DependencyDotNet.Visitors;

namespace DependencyDotNet
{

	internal class Program
	{
		static void Main(string[] args)
		{
			Parser.Default
				.ParseArguments<Options>(args)
				.WithParsed(Handle);
		}

		private static void Handle(Options opts)
		{
			var node = new GraphBuilder
				{
					Folders = opts.Directories.ToList(),
					CollapseAssemblies = opts.CollapseAssemblies.ToList(),
					ExpandAssemblies = opts.ExpandAssemblies.ToList(),
				}
				.Build(opts.File);

			node = GetVisitors(opts)
				.Aggregate(node, (current, visitor) => visitor.Visit(current));

			Save(opts, node);
		}

		private static IEnumerable<GraphVisitor> GetVisitors(Options opts)
		{
			if (opts.FindPathTo != null && opts.FindPathTo.Any())
				yield return new FindPathToVisitor(opts.FindPathTo);

			if (opts.FilterAssemblies != null && opts.FilterAssemblies.Any())
				yield return new FilterVisitor(opts.FilterAssemblies);
		}

		private static void Save(Options opts, GraphNode node)
		{
			if (opts.OutputeFile != null)
			{
				using (var stream = File.Open(opts.OutputeFile, FileMode.Create))
					Save(new StreamWriter(stream, Encoding.UTF8), node);
			}
			else
				Save(Console.Out, node);
		}

		private static void Save(TextWriter writer, GraphNode node)
		{
			using (var xmlWriter = new XmlTextWriter(writer)
			{
				IndentChar = ' ',
				Indentation = 2,
				Formatting = Formatting.Indented
			})
			{
				new SaveVisitor(xmlWriter).Visit(node);
			}
		}
	}
}
