using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;
using Untech.AsyncCommandEngine.Transports;


namespace Untech.AsyncCommandEngine
{
	using MiddlewareCreator = Func<IEngineBuilderContext, IRequestProcessorMiddleware>;

	/// <summary>
	/// Used for <see cref="IOrchestrator"/> and <see cref="IRequestProcessor"/> configuration.
	/// </summary>
	public class EngineBuilder : IEngineBuilderContext
	{
		private readonly List<MiddlewareCreator> _middlewareCreators = new List<MiddlewareCreator>();

		private ITransport _transport;
		private ILoggerFactory _loggerFactory;
		private IRequestMetadataProvider _requestMetadataProvider;

		/// <summary>
		/// Sets <see cref="ILoggerFactory"/> that will be used for logging.
		/// </summary>
		/// <param name="loggerFactory">The logger factory.</param>
		/// <returns></returns>
		public EngineBuilder LogTo(ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory;
			return this;
		}

		/// <summary>
		/// Sets <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="transport">The transport to use.</param>
		/// <returns></returns>
		public EngineBuilder ReceiveRequestsFrom(ITransport transport)
		{
			_transport = transport;
			return this;
		}

		/// <summary>
		/// Sets <see cref="IRequestMetadataProvider"/> that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="provider">The provider to use.</param>
		/// <returns></returns>
		public EngineBuilder ReadMetadataFrom(IRequestMetadataProvider provider)
		{
			_requestMetadataProvider = provider;
			return this;
		}

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="creator">The function that creates <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public EngineBuilder Then(Func<IEngineBuilderContext, IRequestProcessorMiddleware> creator)
		{
			_middlewareCreators.Add(creator);
			return this;
		}


		/// <inheritdoc />
		public ILoggerFactory GetLogger()
		{
			return _loggerFactory ?? NullLoggerFactory.Instance;
		}

		/// <inheritdoc />
		public ITransport GetTransport()
		{
			return _transport;
		}

		/// <inheritdoc />
		public IRequestMetadataProvider GetMetadata()
		{
			return _requestMetadataProvider ?? NullRequestMetadataProvider.Instance;
		}

		/// <summary>
		/// Returns constructed instance of the <see cref="IRequestProcessor"/>.
		/// </summary>
		/// <param name="strategy">The CQRS strategy that will be used during request execution.</param>
		/// <returns></returns>
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

		/// <summary>
		/// Returns constructed instance of the <see cref="IRequestProcessor"/>.
		/// </summary>
		/// <param name="strategy">The CQRS strategy that will be used during request execution.</param>
		/// <returns></returns>
		public IRequestProcessor BuildProcessor(Func<IEngineBuilderContext, ICqrsStrategy> strategy)
		{
			return BuildProcessor(strategy(this));
		}

		/// <summary>
		/// Returns constructed instance of the <see cref="IOrchestrator"/>.
		/// </summary>
		/// <param name="strategy">The CQRS strategy that will be used during request execution.</param>
		/// <param name="options">The options for orchestrator configuration.</param>
		/// <returns></returns>
		public IOrchestrator BuildOrchestrator(ICqrsStrategy strategy, OrchestratorOptions options)
		{
			EnsureOptionsValid(options);

			return new Orchestrator(options,
				GetTransport(),
				GetMetadata(),
				BuildProcessor(strategy),
				GetLogger().CreateLogger<Orchestrator>());
		}

		/// <summary>
		/// Returns constructed instance of the <see cref="IOrchestrator"/>.
		/// </summary>
		/// <param name="strategy">The CQRS strategy that will be used during request execution.</param>
		/// <param name="options">The options for orchestrator configuration.</param>
		/// <returns></returns>
		public IOrchestrator BuildOrchestrator(Func<IEngineBuilderContext, ICqrsStrategy> strategy,
			OrchestratorOptions options)
		{
			return BuildOrchestrator(strategy(this), options);
		}

		/// <summary>
		/// Ensures that options valid. Uses <see cref="Validator"/> for validation.
		/// </summary>
		/// <param name="options"></param>
		public void EnsureOptionsValid(object options)
		{
			var validationContext = new ValidationContext(options);
			Validator.ValidateObject(options, validationContext, true);
		}
	}
}