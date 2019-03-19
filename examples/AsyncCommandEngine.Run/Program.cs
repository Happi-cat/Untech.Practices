using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace AsyncCommandEngine.Run
{
	public class AceRequest
	{
		public AceRequest(string commandName, ICommandTypeMetadata typeMetadata)
		{
			CommandName = commandName;
			CommandTypeMetadata = typeMetadata;
		}

		public string CommandName { get; private set; }
		public ICommandTypeMetadata CommandTypeMetadata { get; private set; }
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

	public class DummyAceProcessorMiddleware : IAceProcessorMiddleware
	{
		public async Task Execute(AceContext context, AceRequestProcessorDelegate next)
		{
			context.RequestAborted.ThrowIfCancellationRequested();

			await next(context);
		}
	}

	public class WatchDogProcessorMiddleware : IAceProcessorMiddleware
	{
		private readonly WatchDogOptions _options;

		public WatchDogProcessorMiddleware(WatchDogOptions options)
		{
			_options = options;
		}

		public async Task Execute(AceContext context, AceRequestProcessorDelegate next)
		{
			var timeout = GetTimeout(context);
			if (timeout != null)
			{
				var timeoutTokenSource = new CancellationTokenSource(timeout.Value);
				var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
					timeoutTokenSource.Token,
					context.RequestAborted);

				context.RequestAborted = linkedTokenSource.Token;
			}

			await next(context);
		}

		private TimeSpan? GetTimeout(AceContext context)
		{
			var attr = context.Request.CommandTypeMetadata.GetAttribute<WatchDogOptionsAttribute>();

			if (attr != null) return attr.Timeout;
			if (_options.CommandTimeouts.TryGetValue(context.Request.CommandName, out var timeout)) return timeout;
			return _options.DefaultTimeout;
		}
	}

	public class DummyCommand : ICommand<int>
	{
	}

	[WatchDogOptions(0, 10, 0)]
	public class DummyCommandHandler: ICommandHandler<DummyCommand, int>
	{
		public Task<int> HandleAsync(DummyCommand request, CancellationToken cancellationToken)
		{
			Console.WriteLine("Hello World!");
			return Task.FromResult(100);
		}
	}


	public class DebounceOptions{ }

	public class ThrottleOptions
	{
		public int? DefaultRunAtOnce { get; set; }


	}

	public interface ICommandTypeMetadata
	{
		Type Type { get; }

		TAttr GetAttribute<TAttr>() where TAttr: Attribute;
	}

	public class CommandTypeMetadata<T> : ICommandTypeMetadata
	{
		private static readonly Type s_type = typeof(T);

		public Type Type => s_type;

		public TAttr GetAttribute<TAttr>() where TAttr:Attribute
		{
			return AttrCache<TAttr>.Attributes.FirstOrDefault();
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

	[AttributeUsage(AttributeTargets.Class)]
	public class WatchDogOptionsAttribute : Attribute
	{
		private readonly int _hours;
		private readonly int _minutes;
		private readonly int _seconds;

		public WatchDogOptionsAttribute(int hours, int minutes, int seconds)
		{
			_hours = hours;
			_minutes = minutes;
			_seconds = seconds;
		}

		public TimeSpan Timeout => new TimeSpan(_hours, _minutes, _seconds);
	}

	public class WatchDogOptions
	{
		/// <summary>
		/// Null is for disabled default timeout
		/// </summary>
		public TimeSpan? DefaultTimeout { get; set; }

		public ReadOnlyDictionary<string, TimeSpan> CommandTimeouts { get; set; }
	}

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

		public AceProcessor(IEnumerable<IAceProcessorMiddleware> middlewares)
		{
			_middlewares = middlewares.ToList();
		}

		public Task Execute(AceContext context)
		{
			return GetNext(0)(context);
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
					CommandTimeouts = new ReadOnlyDictionary<string, TimeSpan>(new Dictionary<string, TimeSpan>
					{
						["Library1.Command1"] = TimeSpan.FromMinutes(10)
					})
				})
				.BuildService();


			service.Execute(new AceContext(new AceRequest("test", new CommandTypeMetadata<DummyCommandHandler>())));

		}
	}
}