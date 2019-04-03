using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine
{
	public class EngineBuilder
	{
		private readonly List<IRequestProcessorMiddleware> _middlewares = new List<IRequestProcessorMiddleware>();
		private ITransport _transport;
		private ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;
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

		public EngineBuilder Use(Func<IRequestProcessorMiddleware> creator)
		{
			_middlewares.Add(creator());
			return this;
		}

		public IRequestProcessor BuildProcessor(ICqrsStrategy strategy)
		{
			var predefinedMiddleware = new IRequestProcessorMiddleware[]
			{
				new CqrsMiddleware(strategy),
			};

			return new RequestProcessor(_middlewares.Concat(predefinedMiddleware));
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