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
				.MapResult<InitOptions, NewOptions, ListOptions, int>(Init, New, List, errs => 1);
		}

		private static int Init(InitOptions opts)
		{
			if (!Directory.Exists(opts.Directory)) Directory.CreateDirectory(opts.Directory);

			var filePath = AdrEntry.CreateInitial().Save(opts.Directory);
			Console.WriteLine("Initialized and created {0}", filePath);
			return 0;
		}

		private static int New(NewOptions opts)
		{
			var filePath = AdrEntry.CreateNew(opts.Title).Save(opts.Directory);
			Console.WriteLine("Created {0}", filePath);
			return 0;
		}

		private static int List(ListOptions opts)
		{
			return 0;
		}
	}
}