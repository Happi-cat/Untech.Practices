using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DependencyDotNet
{
	public class DependencyGraphBuilder
	{
		private static readonly IReadOnlyList<string> s_excludeAssemblies = new List<string>
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

		public List<string> CollapseAssemblies { get; set; }

		public List<string> ExpandAssemblies { get; set; }

		public List<string> Folders { get; set; }

		public List<string> FilterAssemblies { get; set; }

		public DependencyGraphNode Build(Assembly assembly)
		{
			return Build(assembly.GetName(), assembly);
		}

		public DependencyGraphNode Build(string filePath)
		{
			if (!File.Exists(filePath))
				throw new ArgumentException(nameof(filePath));

			var assembly = Assembly.LoadFrom(filePath);
			Folders = Folders ?? new List<string>();
			Folders.Add(Path.GetDirectoryName(assembly.Location));

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
			return ShouldBuildWithDependencies(assemblyName)
				? BuildWithDependencies(assemblyName, assembly ?? Find(assemblyName))
				: GetMemoized(assemblyName, () => new DependencyGraphNode(assemblyName));
		}

		private DependencyGraphNode BuildWithDependencies(AssemblyName assemblyName, Assembly assembly)
		{
			return assembly == null
				? GetMemoized(assemblyName, () => new DependencyGraphNode(assemblyName) { NotFound = true })
				: GetMemoized(assemblyName, () => new DependencyGraphNode(assemblyName)
				{
					References = assembly
						.GetReferencedAssemblies()
						.Where(ShouldAddAsDependency)
						.Select(referenceAssemblyName => Build(referenceAssemblyName))
						.ToList()
				});
		}

		private bool ShouldAddAsDependency(AssemblyName assemblyName)
		{
			return FilterAssemblies == null
				|| FilterAssemblies.Count == 0
				|| FilterAssemblies.Any(mask => Wildcard.IsMatch(assemblyName.Name, mask));
		}

		private bool ShouldBuildWithDependencies(AssemblyName assemblyName)
		{
			var name = assemblyName.Name;

			if (ExpandAssemblies?.Any(mask => Wildcard.IsMatch(name, mask)) == true)
				return true;

			if (s_excludeAssemblies.Any(nameOrMask => Wildcard.IsMatch(name, nameOrMask)))
				return false;
			if (CollapseAssemblies?.Any(nameOrMask => Wildcard.IsMatch(name, nameOrMask)) == true)
				return false;

			return true;
		}

		private Assembly Find(AssemblyName assemblyName)
		{
			if (Folders != null)
			{
				var fileThatExists = Folders
					.Select(dir => Path.Combine(dir, assemblyName.Name + ".dll"))
					.FirstOrDefault(File.Exists);

				if (fileThatExists != null)
					return Assembly.LoadFrom(fileThatExists);
			}

			try { return Assembly.Load(assemblyName.FullName); }
			catch { return null; }
		}
	}
}
