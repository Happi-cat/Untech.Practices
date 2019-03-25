using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Untech.Practices.CQRS.Handlers;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class BuiltInRequestMetadataAccessors : IRequestMetadataAccessors
	{
		private readonly IReadOnlyDictionary<string, IRequestMetadataAccessor> _requestsMetadata;

		public BuiltInRequestMetadataAccessors(Assembly[] assemblies)
		{
			_requestsMetadata = CollectRequestsMetadata(assemblies);
		}

		private static Dictionary<string, IRequestMetadataAccessor> CollectRequestsMetadata(Assembly[] assemblies)
		{
			var requestsMetadata = new Dictionary<string, IRequestMetadataAccessor>();
			var typeDetectives = assemblies
				.SelectMany(a => a.DefinedTypes)
				.Where(a => a.IsPublic)
				.Select(t => new TypeDetective(t));

			foreach (var typeDetective in typeDetectives)
			foreach (var requestType in typeDetective.GetSupportableRequestTypes())
			{
				if (string.IsNullOrEmpty(requestType.FullName)) continue;
				if (requestsMetadata.ContainsKey(requestType.FullName))
					throw new InvalidOperationException($"Found several handlers for {requestType}");

				requestsMetadata.Add(requestType.FullName, typeDetective.GetMetadata());
			}

			return requestsMetadata;
		}

		public IRequestMetadataAccessor GetMetadata(string requestName)
		{
			return _requestsMetadata.TryGetValue(requestName, out var requestMetadata)
				? requestMetadata
				: new NullRequestMetadataAccessor();
		}

		private class TypeDetective
		{
			private static readonly Type s_genericCommandHandlerType = typeof(ICommandHandler<,>);
			private static readonly Type s_genericRequestMetadataType = typeof(IRequestMetadataSource<>);

			private readonly TypeInfo _suspectedType;
			private readonly Type[] _supportableRequests;

			private IRequestMetadataAccessor _metadataAccessor;

			public TypeDetective(TypeInfo suspectedType)
			{
				var matchableGenericTypes = new[]
				{
					s_genericCommandHandlerType,
					s_genericRequestMetadataType
				};

				_suspectedType = suspectedType;
				_supportableRequests = _suspectedType
					.ImplementedInterfaces
					.Where(ifType => ifType.IsConstructedGenericType
						&& matchableGenericTypes.Contains(ifType.GetGenericTypeDefinition()))
					.Select(ifType => ifType.GenericTypeArguments[0])
					.ToArray();
			}

			public IEnumerable<Type> GetSupportableRequestTypes()
			{
				return _supportableRequests;
			}

			public IRequestMetadataAccessor GetMetadata()
			{
				if (_metadataAccessor != null) return _metadataAccessor;

				if (_supportableRequests.Length == 0)
				{
					_metadataAccessor = new NullRequestMetadataAccessor();
				}
				else
				{
					var accessorType =typeof(RequestMetadataAccessor<>).MakeGenericType(_suspectedType.AsType());
					_metadataAccessor = (IRequestMetadataAccessor)Activator.CreateInstance(accessorType);
				}

				return _metadataAccessor;
			}
		}

		private class RequestMetadataAccessor<TMetadataContainer> : IRequestMetadataAccessor
		{
			private static readonly Type s_typeMetadataContainer = typeof(TMetadataContainer);

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
					Attributes = new ReadOnlyCollection<TAttr>(s_typeMetadataContainer
						.GetTypeInfo()
						.GetCustomAttributes<TAttr>()
						.ToList());
				}
			}
		}

		private class NullRequestMetadataAccessor : IRequestMetadataAccessor
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