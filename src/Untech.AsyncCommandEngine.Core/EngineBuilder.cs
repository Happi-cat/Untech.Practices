using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;
using Untech.AsyncCommandEngine.Transports;

namespace Untech.AsyncCommandEngine
{
	public interface IBuilderContext
	{
		ILoggerFactory GetLogger();
	}

	public class EngineBuilder : IBuilderContext
	{
		private readonly List<Func<IBuilderContext, IRequestProcessorMiddleware>> _middlewares = new List<Func<IBuilderContext, IRequestProcessorMiddleware>>();
		private ITransport _transport;
		private ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;
		private IRequestMetadataProvider _requestMetadataProvider = NullRequestMetadataProvider.Instance;

		public EngineBuilder UseLogger(ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory;
			return this;
		}

		public EngineBuilder UseTransport(ITransport transport)
		{
			_transport = transport;
			return this;
		}

		public EngineBuilder UseMetadata(IRequestMetadataProvider provider)
		{
			_requestMetadataProvider = provider;
			return this;
		}

		public EngineBuilder Use(Func<IBuilderContext, IRequestProcessorMiddleware> creator)
		{
			_middlewares.Add(creator);
			return this;
		}

		public EngineBuilder Use(Func<Context, RequestProcessorCallback, Task> middleware)
		{
			return Use(ctx => new AdHocRequestProcessorMiddleware(middleware));
		}

		public EngineBuilder Use(Func<IBuilderContext, Func<Context, RequestProcessorCallback, Task>> creator)
		{
			return Use(ctx => new AdHocRequestProcessorMiddleware(creator(ctx)));
		}

		public ILoggerFactory GetLogger()
		{
			return _loggerFactory;
		}

		public ITransport GetTransport()
		{
			return _transport;
		}

		public IRequestMetadataProvider GetMetadataProvider()
		{
			return _requestMetadataProvider;
		}

		public IRequestProcessor BuildProcessor(ICqrsStrategy strategy)
		{
			var middlewares = _middlewares
				.Select(n => n.Invoke(this))
				.Concat(new IRequestProcessorMiddleware[]
				{
					new CqrsMiddleware(strategy),
				});

			return new RequestProcessor(middlewares);
		}

		public IOrchestrator BuildOrchestrator(ICqrsStrategy strategy, OrchestratorOptions options)
		{
			return new Orchestrator(options,
				_transport,
				_requestMetadataProvider ?? NullRequestMetadataProvider.Instance,
				BuildProcessor(strategy),
				_loggerFactory.CreateLogger<Orchestrator>());
		}
	}
}