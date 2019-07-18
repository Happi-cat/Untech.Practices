using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Metadata.Annotations;
using Untech.Practices.CQRS.Handlers;

namespace Untech.AsyncJob.Features.CQRS
{
	/// <summary>
	/// Implements <see cref="IRequestMetadataProvider"/> and can be used for getting builtin attributes.
	/// This attributes can be defined on <see cref="IRequestMetadataSource{TRequest}"/>
	/// and <see cref="ICommandHandler{TIn,TOut}"/>.
	/// </summary>
	public class BuiltInRequestMetadataProvider : IRequestMetadataProvider
	{
		private readonly IReadOnlyDictionary<string, IRequestMetadata> _requestsMetadata;

		public BuiltInRequestMetadataProvider(Assembly assembly)
			: this(new [] { assembly })
		{
		}

		public BuiltInRequestMetadataProvider(IEnumerable<Assembly> assemblies)
		{
			if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

			_requestsMetadata = CollectRequestsMetadata(assemblies);
		}

		/// <inheritdoc />
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
			{
				requestsMetadata.AddRange(typeExplorer.GetSupportableRequestTypes()
					.Where(requestType => !string.IsNullOrEmpty(requestType.FullName))
					.Select(requestType => (requestType.FullName, typeExplorer.GetMetadata())));
			}

			return requestsMetadata
				.GroupBy(n => n.FullName, n => n.Accessor)
				.ToDictionary(n => n.Key, n => (IRequestMetadata)new CompositeRequestMetadata(n));
		}

		private class TypeExplorer
		{
			private static readonly IReadOnlyList<Type> s_matchableGenericTypes = new List<Type>
			{
				typeof(ICommandHandler<,>)
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
				if (_supportableRequests.Count == 0) return NullRequestMetadata.Instance;

				var accessorType = typeof(TypeMetadata<>).MakeGenericType(_suspectedType.AsType());
				return (IRequestMetadata)Activator.CreateInstance(accessorType);
			}
		}

		private class TypeMetadata<TContainer> : IRequestMetadata
		{
			private static readonly TypeInfo s_metadataContainerType = typeof(TContainer).GetTypeInfo();

			public TAttr GetAttribute<TAttr>() where TAttr : MetadataAttribute
			{
				return Attributes<TAttr>.All.SingleOrDefault();
			}

			public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : MetadataAttribute
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
