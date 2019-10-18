using System;
using System.IO;
using Adr.Options;
using CommandLine;

namespace Adr
{
	internal static class Program
	{
		private static int Main(string[] args)
		{
			return Parser.Default
				.ParseArguments<InitOptions, NewOptions, ListOptions>(args)
				.MapResult<InitOptions, NewOptions, ListOptions, int>(
					Init, New, List, errs => 1
				);
		}

		private static int Init(InitOptions opts)
		{
			if (!Directory.Exists(opts.Directory)) Directory.CreateDirectory(opts.Directory);

			var file = AdrFile.Save(new AdrDirectory(opts.Directory), AdrEntry.CreateInitial());
			Console.WriteLine("Initialized and created {0}", file.FilePath);
			return 0;
		}

		private static int New(NewOptions opts)
		{
			var directory = new AdrDirectory(opts.Directory);
			var newEntry = AdrEntry.CreateNew(opts.Title);
			var current = AdrFile.Save(directory, newEntry);
			Console.WriteLine("Created {0}", current.FilePath);

			if (opts.SupersedesAdr != null)
			{
				var referenced = directory.GetRecord(opts.SupersedesAdr.Value);
				if (referenced != null)
				{
					current.Entry.AppendStatus($"Supersedes {referenced.AsLink()}");
					referenced.Entry.AppendStatus($"Superseded by {current.AsLink()}");
					current.Save();
					referenced.Save();
				}
				else
				{
					Console.WriteLine("{0} not found", opts.SupersedesAdr);
					return 1;
				}
			}

			if (opts.AmendsAdr != null)
			{
				var referenced = directory.GetRecord(opts.AmendsAdr.Value);
				if (referenced != null)
				{
					current.Entry.AppendStatus($"Amends {referenced.AsLink()}");
					referenced.Entry.AppendStatus($"Amended by {current.AsLink()}");
					current.Save();
					referenced.Save();
				}
				else
				{
					Console.WriteLine("{0} not found", opts.AmendsAdr);
					return 1;
				}
			}

			return 0;
		}

		private static int List(ListOptions opts)
		{
			foreach (var file in new AdrDirectory(opts.Directory).GetRecords())
			{
				Console.WriteLine("{0,4} {1,-40} {2,8:yy-MM-dd} {3,-10}",
					file.Entry.Number,
					file.Entry.Title,
					file.Entry.When,
					file.Entry.GetShortStatus());
			}

			return 0;
		}
	}
}