using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Untech.AsyncCommmandEngine.Abstractions;
using Untech.Practices.CQRS.Handlers;

namespace AsyncCommandEngine.Run
{
	internal class RequestMetadataProvider
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
				.SelectMany(a => a.GetTypes())
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

			private readonly Type _type;
			private readonly Type[] _supportableCommands;

			private IRequestMetadata _metadata;

			public TypeDetective(Type type)
			{
				_type = type;
				_supportableCommands = _type
					.GetInterfaces()
					.Where(ifType => ifType.IsGenericType
						&& ifType.GetGenericTypeDefinition() == s_genericCommandHandlerType)
					.Select(ifType => ifType.GetGenericArguments()[0])
					.ToArray();
			}

			public Type SuspectedType => _type;

			public IEnumerable<Type> GetCommandTypes()
			{
				return _supportableCommands;
			}

			public IRequestMetadata GetMetadata()
			{
				if (_metadata == null)
				{
					var requestMetadataType = typeof(RequestMetadata<>).MakeGenericType(_type);
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
					Attributes = new ReadOnlyCollection<TAttr>(s_type.GetCustomAttributes<TAttr>().ToList());
				}
			}
		}

		private class NullRequestMetadata : IRequestMetadata
		{
			public TAttr GetAttribute<TAttr>() where TAttr : Attribute
			{
				return default(TAttr);
			}

			public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
			{
				return Enumerable.Empty<TAttr>();
			}
		}
	}
}