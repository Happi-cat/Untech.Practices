using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AsyncCommandEngine.Examples;
using AsyncCommandEngine.Examples.Features.Throttling;
using AsyncCommandEngine.Run;
using Microsoft.Extensions.Hosting;
using Untech.AsyncCommmandEngine.Abstractions;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace Untech.AsyncCommmandEngine.Abstractions
{
	// used for request specific options/configuration
	public interface IRequestTypeMetadataAccessor
	{
		TAttr GetAttribute<TAttr>() where TAttr: Attribute;
		IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr: Attribute;
	}

	public class AceRequest
	{
		public AceRequest(string typeName, DateTimeOffset created, IRequestTypeMetadataAccessor typeMetadataAccessor)
		{
			TypeName = typeName;
			Created = created;
			TypeMetadataAccessor = typeMetadataAccessor;
		}

		public string TypeName { get; private set; }
		public IRequestTypeMetadataAccessor TypeMetadataAccessor { get; private set; }
		public DateTimeOffset Created { get; private set; }
		public string Body { get; private set; }
	}

	public class AceContext
	{
		public AceContext(AceRequest request)
		{
			Request = request;
			Items = new Dictionary<object, object>();
		}

		public AceRequest Request { get; private set; }

		public CancellationToken RequestAborted { get; set; }
		public IDictionary<object, object> Items { get; set; }
	}

	public delegate Task AceRequestProcessorDelegate(AceContext context);

	public interface IAceProcessorMiddleware
	{
		Task Execute(AceContext context, AceRequestProcessorDelegate next);
	}
}

namespace AsyncCommandEngine.Run
{
	public class RequestTypeMetadataAccessor<T> : IRequestTypeMetadataAccessor
	{
		private static readonly Type s_type = typeof(T);

		public Type Type => s_type;

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

	public class NullRequestTypeMetadataAccessor : IRequestTypeMetadataAccessor
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




	public class DebounceOptions{ }






	public class AceBuilder
	{
		private readonly List<IAceProcessorMiddleware> _middlewares = new List<IAceProcessorMiddleware>();

		public AceBuilder UseTransport()
		{
			throw new NotImplementedException();
		}

		public AceBuilder UseDebounce(DebounceOptions debounceOptions)
		{
			throw new NotImplementedException();
		}

		public AceBuilder UseThottling(ThrottleOptions throttleOptions)
		{
			throw new NotImplementedException();
		}

		public AceBuilder UseWatchDog(WatchDogOptions watchDogOptions)
		{
			_middlewares.Add(new WatchDogProcessorMiddleware(watchDogOptions));
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

	public class AceProcessor
	{
		private readonly IReadOnlyList<IAceProcessorMiddleware> _middlewares;
		private AceRequestProcessorDelegate _cachedNext;

		public AceProcessor(IEnumerable<IAceProcessorMiddleware> middlewares)
		{
			_middlewares = middlewares.ToList();
		}

		public Task Execute(AceContext context)
		{
			if (_cachedNext == null) _cachedNext = GetNext(0);

			return _cachedNext(context);
		}

		private AceRequestProcessorDelegate GetNext(int nextState)
		{
			return ctx =>
			{
				if (nextState >= _middlewares.Count) throw NextWasCalledAfterLastMiddleware();

				var middleware = _middlewares[nextState];
				Console.WriteLine("Middleware: {0}", middleware.GetType());
				return middleware.Execute(ctx, GetNext(nextState + 1));
			};
		}

		private Exception NextWasCalledAfterLastMiddleware()
		{
			return new InvalidOperationException("next was called inside final middle.");
		}
	}


	class Program
	{
		static void Main(string[] args)
		{
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


			service.Execute(new AceContext(new AceRequest("test", DateTimeOffset.UtcNow, new RequestTypeMetadataAccessor<DummyCommandHandler>())));

		}
	}
}