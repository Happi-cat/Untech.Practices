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
			if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

			_requestsMetadata = CollectRequestsMetadata(assemblies);
		}

		public IRequestMetadata GetMetadata(string requestName)
		{
			if (string.IsNullOrEmpty(requestName)) throw new ArgumentNullException(nameof(requestName));

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
				.Select(t => new TypeExplorer(t));

			var requestsMetadata = new List<(string FullName, IRequestMetadata Accessor)>();
			foreach (TypeExplorer typeExplorer in typeDetectives)
			foreach (Type requestType in typeExplorer.GetSupportableRequestTypes())
			{
				if (string.IsNullOrEmpty(requestType.FullName)) continue;
				requestsMetadata.Add((requestType.FullName, typeExplorer.GetMetadata()));
			}

			return requestsMetadata
				.GroupBy(n => n.FullName, n => n.Accessor)
				.ToDictionary(n => n.Key, n => (IRequestMetadata)new CompositeRequestMetadata(n));
		}

		private class TypeExplorer
		{
			private static readonly IReadOnlyList<Type> s_matchableGenericTypes = new List<Type>
			{
				typeof(ICommandHandler<,>), typeof(IRequestMetadataSource<>)
			};

			private readonly TypeInfo _suspectedType;
			private readonly IReadOnlyList<Type> _supportableRequests;

			private IRequestMetadata _metadata;

			public TypeExplorer(TypeInfo suspectedType)
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

				var accessorType = typeof(Metadata<>).MakeGenericType(_suspectedType.AsType());
				return (IRequestMetadata)Activator.CreateInstance(accessorType);
			}
		}

		private class Metadata<TContainer> : IRequestMetadata
		{
			private static readonly TypeInfo s_metadataContainerType = typeof(TContainer).GetTypeInfo();

			public TAttr GetAttribute<TAttr>() where TAttr : Attribute
			{
				return Attributes<TAttr>.All.SingleOrDefault();
			}

			public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
			{
				return Attributes<TAttr>.All;
			}

			private struct Attributes<TAttr> where TAttr : Attribute
			{
				internal static readonly ReadOnlyCollection<TAttr> All;

				static Attributes()
				{
					All = s_metadataContainerType
						.GetCustomAttributes<TAttr>()
						.ToList()
						.AsReadOnly();
				}
			}
		}
	}
}