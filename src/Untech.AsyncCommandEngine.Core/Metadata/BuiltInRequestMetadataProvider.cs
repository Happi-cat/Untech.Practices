using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Untech.Practices.CQRS.Handlers;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class BuiltInRequestMetadataProvider : IRequestMetadataProvider
	{
		private readonly IReadOnlyDictionary<string, IRequestMetadata> _requestsMetadata;

		public BuiltInRequestMetadataProvider(IEnumerable<Assembly> assemblies)
		{
			_requestsMetadata = CollectRequestsMetadata(assemblies);
		}

		public IRequestMetadata GetMetadata(string requestName)
		{
			return _requestsMetadata.TryGetValue(requestName, out var requestMetadata)
				? requestMetadata
				: NullRequestMetadata.Instance;
		}

		private static Dictionary<string, IRequestMetadata> CollectRequestsMetadata(
			IEnumerable<Assembly> assemblies)
		{
			var typeDetectives = assemblies
				.SelectMany(a => a.DefinedTypes)
				.Where(a => a.IsPublic)
				.Select(t => new TypeDetective(t));

			var requestsMetadata = new List<(string FullName, IRequestMetadata Accessor)>();
			foreach (TypeDetective typeDetective in typeDetectives)
			foreach (Type requestType in typeDetective.GetSupportableRequestTypes())
			{
				if (string.IsNullOrEmpty(requestType.FullName)) continue;
				requestsMetadata.Add((requestType.FullName, typeDetective.GetMetadata()));
			}

			return requestsMetadata
				.GroupBy(n => n.FullName, n => n.Accessor)
				.ToDictionary(n => n.Key, n => (IRequestMetadata)new CompositeRequestMetadata(n));
		}

		private class TypeDetective
		{
			private static readonly IReadOnlyList<Type> s_matchableGenericTypes = new List<Type>
			{
				typeof(ICommandHandler<,>), typeof(IRequestMetadataSource<>)
			};

			private readonly TypeInfo _suspectedType;
			private readonly IReadOnlyList<Type> _supportableRequests;

			private IRequestMetadata _metadata;

			public TypeDetective(TypeInfo suspectedType)
			{
				_suspectedType = suspectedType;
				_supportableRequests = _suspectedType.ImplementedInterfaces
					.Where(ifType => ifType.IsConstructedGenericType
						&& s_matchableGenericTypes.Contains(ifType.GetGenericTypeDefinition()))
					.Select(ifType => ifType.GenericTypeArguments[0])
					.ToList();
			}

			public IEnumerable<Type> GetSupportableRequestTypes()
			{
				return _supportableRequests;
			}

			public IRequestMetadata GetMetadata()
			{
				if (_metadata != null) return _metadata;

				return _metadata = CreateMetadata();
			}

			private IRequestMetadata CreateMetadata()
			{
				if (_supportableRequests.Count == 0)
					return NullRequestMetadata.Instance;

				var accessorType = typeof(RequestMetadata<>).MakeGenericType(_suspectedType.AsType());
				return (IRequestMetadata)Activator.CreateInstance(accessorType);
			}
		}

		private class RequestMetadata<TMetadataContainer> : IRequestMetadata
		{
			private static readonly TypeInfo s_metadataContainerType = typeof(TMetadataContainer).GetTypeInfo();

			public TAttr GetAttribute<TAttr>() where TAttr : Attribute
			{
				return AttrCache<TAttr>.Attributes.SingleOrDefault();
			}

			public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
			{
				return AttrCache<TAttr>.Attributes;
			}

			private struct AttrCache<TAttr> where TAttr : Attribute
			{
				internal static readonly ReadOnlyCollection<TAttr> Attributes;

				static AttrCache()
				{
					Attributes = s_metadataContainerType
						.GetCustomAttributes<TAttr>()
						.ToList()
						.AsReadOnly();
				}
			}
		}
	}
}