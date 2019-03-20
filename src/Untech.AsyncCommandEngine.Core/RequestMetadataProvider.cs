using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Untech.Practices.CQRS.Handlers;

namespace Untech.AsyncCommandEngine
{
	public class RequestMetadataProvider
	{
		private readonly Assembly[] _assemblies;
		private readonly IReadOnlyDictionary<string, IRequestMetadata> _commandsMetadata;

		public RequestMetadataProvider(Assembly[] assemblies)
		{
			_assemblies = assemblies;
			_commandsMetadata = GetCommandHandlerTypes(assemblies);
		}

		private static Dictionary<string, IRequestMetadata> GetCommandHandlerTypes(Assembly[] assemblies)
		{
			var commandToMetadataMap = new Dictionary<string, IRequestMetadata>();
			var types = assemblies
				.SelectMany(a => a.DefinedTypes)
				.Select(t => new TypeDetective(t));

			foreach (var typeDetective in types)
			{
				foreach (var commandType in typeDetective.GetCommandTypes())
				{
					if (commandToMetadataMap.ContainsKey(commandType.FullName))
					{
						throw new InvalidOperationException($"Found several handlers for {commandType}");
					}

					commandToMetadataMap.Add(commandType.FullName, typeDetective.GetMetadata());
				}
			}

			return commandToMetadataMap;
		}

		public IRequestMetadata GetMetadata(string commandTypeName)
		{
			return _commandsMetadata.TryGetValue(commandTypeName, out var requestMetadata)
				? requestMetadata
				: new NullRequestMetadata();
		}

		private class TypeDetective
		{
			private static readonly Type s_genericCommandHandlerType = typeof(ICommandHandler<,>);

			private readonly TypeInfo _type;
			private readonly Type[] _supportableCommands;

			private IRequestMetadata _metadata;

			public TypeDetective(TypeInfo type)
			{
				_type = type;
				_supportableCommands = _type
					.ImplementedInterfaces
					.Where(ifType => ifType.IsConstructedGenericType
						&& ifType.GetGenericTypeDefinition() == s_genericCommandHandlerType)
					.Select(ifType => ifType.GenericTypeArguments[0])
					.ToArray();
			}

			public TypeInfo SuspectedType => _type;

			public IEnumerable<Type> GetCommandTypes()
			{
				return _supportableCommands;
			}

			public IRequestMetadata GetMetadata()
			{
				if (_metadata == null)
				{
					var requestMetadataType = typeof(RequestMetadata<>).MakeGenericType(_type.AsType());
					_metadata = (IRequestMetadata)Activator.CreateInstance(requestMetadataType);
				}

				return _metadata;
			}
		}

		private class RequestMetadata<T> : IRequestMetadata
		{
			private static readonly Type s_type = typeof(T);

			public TAttr GetAttribute<TAttr>() where TAttr:Attribute
			{
				return AttrCache<TAttr>.Attributes.FirstOrDefault();
			}

			public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
			{
				return AttrCache<TAttr>.Attributes;
			}

			private struct AttrCache<TAttr> where TAttr: Attribute
			{
				public static readonly ReadOnlyCollection<TAttr> Attributes;

				static AttrCache()
				{
					Attributes = new ReadOnlyCollection<TAttr>(s_type
						.GetTypeInfo()
						.GetCustomAttributes<TAttr>()
						.ToList());
				}
			}
		}

		private class NullRequestMetadata : IRequestMetadata
		{
			public TAttr GetAttribute<TAttr>() where TAttr : Attribute
			{
				return default;
			}

			public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
			{
				return Enumerable.Empty<TAttr>();
			}
		}
	}
}