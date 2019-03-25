using System;
using System.Collections.Generic;
using System.Linq;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine
{
	public class EngineBuilder
	{
		private readonly List<IRequestProcessorMiddleware> _middlewares = new List<IRequestProcessorMiddleware>();
		private ITransport _transport;
		private IRequestMetadataAccessors _requestMetadataAccessors;

		public EngineBuilder UseTransport(ITransport transport)
		{
			_transport = transport;
			return this;
		}

		public EngineBuilder UseMetadata(IRequestMetadataAccessors accessors)
		{
			_requestMetadataAccessors = accessors;
			return this;
		}

		public EngineBuilder Use(Func<IRequestProcessorMiddleware> creator)
		{
			_middlewares.Add(creator());
			return this;
		}

		public IRequestProcessor BuildService(ICqrsStrategy strategy)
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
				_requestMetadataAccessors ?? NullRequestMetadataAccessors.Default,
				BuildService(strategy));
		}
	}
}