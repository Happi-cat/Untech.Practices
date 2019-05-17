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

	public class MiddlewareCollection
	{
		private readonly List<MiddlewareCreator> _middlewareCreators = new List<MiddlewareCreator>();
		private MiddlewareCreator _finalMiddlewareCreator;

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="creator">The function that creates <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public MiddlewareCollection Then(Func<IEngineBuilderContext, IRequestProcessorMiddleware> creator)
		{
			_middlewareCreators.Add(creator);
			return this;
		}

		public MiddlewareCollection Final(ICqrsStrategy strategy)
		{
			_finalMiddlewareCreator = ctx => new CqrsMiddleware(strategy);

			return this;
		}

		public MiddlewareCollection Final(Func<IEngineBuilderContext, ICqrsStrategy> strategy)
		{
			_finalMiddlewareCreator = ctx => new CqrsMiddleware(strategy(ctx));

			return this;
		}

		internal IEnumerable<IRequestProcessorMiddleware> GetMiddlewares(IEngineBuilderContext context)
		{
			if (_finalMiddlewareCreator == null) throw NoFinalStepError();

			return _middlewareCreators
				.Concat(new[] { _finalMiddlewareCreator })
				.Select(n => n.Invoke(context));
		}

		private static InvalidOperationException NoFinalStepError()
		{
			return new InvalidOperationException("Final middleware wasn't configured");
		}
	}

	/// <summary>
	/// Used for <see cref="IOrchestrator"/> and <see cref="IRequestProcessor"/> configuration.
	/// </summary>
	public class EngineBuilder : IEngineBuilderContext
	{
		private readonly MiddlewareCollection _middlewareCollection;

		private ITransport _transport;
		private ILoggerFactory _loggerFactory;
		private IRequestMetadataProvider _requestMetadataProvider;

		public EngineBuilder()
		{
			_middlewareCollection = new MiddlewareCollection();
		}

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

		public EngineBuilder DoSteps(Action<MiddlewareCollection> configureProcessor)
		{
			configureProcessor(_middlewareCollection);

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
		public IRequestProcessor BuildProcessor()
		{
			return new RequestProcessor(_middlewareCollection.GetMiddlewares(this));
		}

		/// <summary>
		/// Returns constructed instance of the <see cref="IOrchestrator"/>.
		/// </summary>
		/// <param name="strategy">The CQRS strategy that will be used during request execution.</param>
		/// <param name="options">The options for orchestrator configuration.</param>
		/// <returns></returns>
		public IOrchestrator BuildOrchestrator(Action<OrchestratorOptions> configureOptions)
		{
			var options = new OrchestratorOptions();
			configureOptions(options);
			EnsureOptionsValid(options);

			return new Orchestrator(options,
				GetTransport(),
				GetMetadata(),
				BuildProcessor(),
				GetLogger().CreateLogger<Orchestrator>());
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