using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;
using Untech.AsyncCommandEngine.Transports;


namespace Untech.AsyncCommandEngine
{
	using MiddlewareCreator = Func<IEngineBuilderContext, IRequestProcessorMiddleware>;

	public class EngineBuilder : IEngineBuilderContext
	{
		private readonly List<MiddlewareCreator> _middlewareCreators = new List<MiddlewareCreator>();

		private ITransport _transport;
		private ILoggerFactory _loggerFactory;
		private IRequestMetadataProvider _requestMetadataProvider;

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

		public EngineBuilder Use(Func<IEngineBuilderContext, IRequestProcessorMiddleware> creator)
		{
			_middlewareCreators.Add(creator);
			return this;
		}

		public ILoggerFactory GetLogger()
		{
			return _loggerFactory ?? NullLoggerFactory.Instance;
		}

		public ITransport GetTransport()
		{
			return _transport;
		}

		public IRequestMetadataProvider GetMetadata()
		{
			return _requestMetadataProvider ?? NullRequestMetadataProvider.Instance;
		}

		public IRequestProcessor BuildProcessor(ICqrsStrategy strategy)
		{
			var steps = _middlewareCreators
				.Select(n => n.Invoke(this))
				.Concat(new IRequestProcessorMiddleware[]
				{
					new CqrsMiddleware(strategy)
				});

			return new RequestProcessor(steps);
		}

		public IOrchestrator BuildOrchestrator(ICqrsStrategy strategy, OrchestratorOptions options)
		{
			return new Orchestrator(options,
				GetTransport(),
				GetMetadata(),
				BuildProcessor(strategy),
				GetLogger().CreateLogger<Orchestrator>());
		}
	}
}