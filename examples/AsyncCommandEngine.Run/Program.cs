using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AsyncCommandEngine.Examples;
using AsyncCommandEngine.Examples.Features.Debounce;
using AsyncCommandEngine.Examples.Features.Throttling;
using Microsoft.Extensions.Hosting;
using Untech.AsyncCommmandEngine.Abstractions;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace Untech.AsyncCommmandEngine.Abstractions
{
	// used for request specific options/configuration
}

namespace AsyncCommandEngine.Run
{
	internal class RequestMetadata<T> : IRequestMetadata
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

	internal class NullRequestMetadata : IRequestMetadata
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
	public class DummyAceProcessorMiddleware : IAceProcessorMiddleware
	{
		public async Task Execute(AceContext context, AceRequestProcessorDelegate next)
		{
			context.RequestAborted.ThrowIfCancellationRequested();

			await next(context);
		}
	}










	public class AceBuilder
	{
		private readonly List<IAceProcessorMiddleware> _middlewares = new List<IAceProcessorMiddleware>();

		public AceBuilder UseTransport()
		{
			throw new NotImplementedException();
		}

		public AceBuilder Use(Func<IAceProcessorMiddleware> creator)
		{
			_middlewares.Add(creator());
			return this;
		}
		public IHostedService Build()
		{
			throw new NotImplementedException();
		}

		internal AceProcessor BuildService()
		{
			return new AceProcessor(_middlewares.Concat(new IAceProcessorMiddleware[]
			{
				new DummyAceProcessorMiddleware(),
				new CqrsMiddleware(null),
			}));
		}
	}

	public class CqrsMiddleware : IAceProcessorMiddleware
	{
		private readonly IDispatcher _dispatcher;

		public CqrsMiddleware(IDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		public async Task Execute(AceContext context, AceRequestProcessorDelegate next)
		{
			await new DummyCommandHandler().HandleAsync(new DummyCommand(), context.RequestAborted);
//			await _dispatcher.ProcessAsync(new DummyCommand(), context.RequestAborted);
		}
	}

	internal class AceProcessor
	{
		private AceRequestProcessorDelegate _next;

		public AceProcessor(IEnumerable<IAceProcessorMiddleware> middlewares)
		{
			_next = GetNext(new Queue<IAceProcessorMiddleware>(middlewares ?? new List<IAceProcessorMiddleware>()));
		}

		public Task Execute(AceContext context)
		{
			return _next(context);
		}

		private static AceRequestProcessorDelegate GetNext(Queue<IAceProcessorMiddleware> middlewares)
		{
			if (middlewares.TryDequeue(out IAceProcessorMiddleware middleware))
			{
				var next = GetNext(middlewares);

				return ctx =>
				{
					Console.WriteLine("Middleware: {0}", middleware.GetType());
					return middleware.Execute(ctx, next);
				};
			}

			return ctx => Task.CompletedTask;
		}
	}

	class RequestMetadataProvider
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
	}


	class Program
	{
		static void Main(string[] args)
		{
			var type = typeof(DummyCommandHandler);

			var metadata = new RequestMetadataProvider(new[] { type.Assembly })
				.GetMetadata(typeof(DummyCommand).FullName);

			var service = new AceBuilder()
//				.UseDebounce(new DebounceOptions())
//				.UseThottling(new ThrottleOptions())
				.UseWatchDog(new WatchDogOptions
				{
					DefaultTimeout = TimeSpan.FromMinutes(1),
					RequestTimeouts = new ReadOnlyDictionary<string, TimeSpan>(new Dictionary<string, TimeSpan>
					{
						["Library1.Command1"] = TimeSpan.FromMinutes(10)
					})
				})
				.BuildService();


			service.Execute(new AceContext(new AceRequest("test", DateTimeOffset.UtcNow, new RequestMetadata<DummyCommandHandler>())));

		}
	}
}