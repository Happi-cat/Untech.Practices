using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.Practices.CQRS;

namespace Untech.AsyncJob.Features.CQRS
{
	public class BuiltInRequestTypeFinder : IRequestTypeFinder
	{
		private readonly IReadOnlyDictionary<string, Type> _types;

		public BuiltInRequestTypeFinder(Assembly assembly)
			: this(new[] { assembly })
		{
		}

		public BuiltInRequestTypeFinder(IEnumerable<Assembly> assemblies)
		{
			if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

			_types = CollectTypes(assemblies);
		}

		public Type FindRequestType(string requestName)
		{
			return _types.TryGetValue(requestName, out var type) ? type : null;
		}

		private static IReadOnlyDictionary<string, Type> CollectTypes(IEnumerable<Assembly> assemblies)
		{
			return assemblies
				.SelectMany(a => a.DefinedTypes)
				.Where(t => t.IsPublic)
				.Where(type => type.ImplementedInterfaces.Any(IsCommandOrEventInterface))
				.ToDictionary(t => t.FullName, t => t.AsType());
		}

		private static bool IsCommandOrEventInterface(Type interfaceType)
		{
			if (interfaceType == typeof(IEvent)) return true;

			if (interfaceType.IsConstructedGenericType)
			{
				var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();

				return genericTypeDefinition == typeof(ICommand<>);
			}

			return false;
		}
	}
}