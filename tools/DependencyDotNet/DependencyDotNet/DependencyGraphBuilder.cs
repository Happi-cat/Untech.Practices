using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DependencyDotNet
{
	public class DependencyGraphBuilder
	{
		private static readonly IReadOnlyList<string> s_notExpandableAssemblies = new List<string>
		{
			"mscorlib",
			"netstandard",
			"System",
			"System.Buffers*",
			"System.Core",
			"System.Collections*",
			"System.ComponentModel*",
			"System.Data*",
			"System.Drawing*",
			"System.Threading*",
			"System.Linq",
			"System.Json",
			"System.Reflection*",
			"System.Resources*",
			"System.Memory*",
			"System.Numerics*",
			"System.Diagnostics*",
			"System.Runtime*",
			"System.Xml*",
			"System.Private*",
			"System.Text*",
			"System.IO*",
			"System.ValueTuple",
		};

		private readonly IDictionary<string, DependencyGraphNode> _nodesCache = new Dictionary<string, DependencyGraphNode>();

		public List<string> NotExpandableAssemblies { get; set; }

		public List<string> Directories { get; set; }

		public DependencyGraphNode Build(Assembly assembly)
		{
			return Build(assembly.GetName(), assembly);
		}

		public DependencyGraphNode Build(string filePath)
		{
			if (!File.Exists(filePath))
				throw new ArgumentException(nameof(filePath));

			var assembly = Assembly.LoadFrom(filePath);
			Directories = Directories ?? new List<string>();
			Directories.Add(assembly.Location);

			return Build(assembly);
		}

		private DependencyGraphNode GetMemoized(AssemblyName assemblyName, Func<DependencyGraphNode> creator)
		{
			return _nodesCache.TryGetValue(assemblyName.FullName, out var value)
				? value
				: _nodesCache[assemblyName.FullName] = creator();
		}

		private DependencyGraphNode Build(AssemblyName assemblyName, Assembly assembly = null)
		{
			var doNotExpand = IsNotExpandable(assemblyName);

			if (doNotExpand)
				return GetMemoized(assemblyName, () => new DependencyGraphNode(assemblyName));

			return BuildWithDependencies(assemblyName, assembly ?? Find(assemblyName));
		}

		private DependencyGraphNode BuildWithDependencies(AssemblyName assemblyName, Assembly assembly)
		{
			if (assembly == null)
				return GetMemoized(assemblyName, () => new DependencyGraphNode(assemblyName));

			return GetMemoized(assemblyName, () => new DependencyGraphNode(assemblyName)
			{
				References = assembly
					.GetReferencedAssemblies()
					.Select(referenceAssemblyName => Build(referenceAssemblyName))
					.ToList()
			});
		}

		private bool IsNotExpandable(AssemblyName assemblyName)
		{
			var name = assemblyName.Name;
			if (s_notExpandableAssemblies.Any(nameOrMask => Wildcard.IsMatch(name, nameOrMask))) return true;
			if (NotExpandableAssemblies != null && NotExpandableAssemblies.Any(nameOrMask => Wildcard.IsMatch(name, nameOrMask))) return true;
			return false;
		}

		private Assembly Find(AssemblyName assemblyName)
		{
			if (Directories != null)
			{
				var fileThatExists = Directories
					.Select(dir => Path.Combine(dir, assemblyName.Name + ".dll"))
					.FirstOrDefault(File.Exists);

				if (fileThatExists != null)
					return Assembly.LoadFrom(fileThatExists);
			}

			try
			{
				return Assembly.Load(assemblyName.FullName);
			}
			catch
			{
				return null;
			}
		}
	}
}
