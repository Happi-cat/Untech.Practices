using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;
using Untech.AsyncCommandEngine.Transports;

namespace Untech.AsyncCommandEngine.Builder
{
	/// <summary>
	/// Used for <see cref="IOrchestrator"/> and <see cref="IRequestProcessor"/> configuration.
	/// </summary>
	public class EngineBuilder : IBuilderContext, IEngineBuilder
	{
		private readonly Action<OrchestratorOptions> _configureOptions;
		private readonly MiddlewareCollection _middlewareCollection;

		private ITransport _transport;
		private ILoggerFactory _loggerFactory;
		private IRequestMetadataProvider _requestMetadataProvider;

		public EngineBuilder()
			:this(_ => {})
		{

		}

		public EngineBuilder(Action<OrchestratorOptions> configureOptions)
		{
			_middlewareCollection = new MiddlewareCollection();
			_configureOptions = configureOptions;
		}

		/// <summary>
		/// Sets <see cref="ILoggerFactory"/> that will be used for logging.
		/// </summary>
		/// <param name="loggerFactory">The logger factory.</param>
		/// <returns></returns>
		public IEngineBuilder LogTo(ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory;
			return this;
		}

		/// <summary>
		/// Sets <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="transport">The transport to use.</param>
		/// <returns></returns>
		public IEngineBuilder ReceiveRequestsFrom(ITransport transport)
		{
			_transport = transport;
			return this;
		}

		/// <summary>
		/// Sets <see cref="IRequestMetadataProvider"/> that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="provider">The provider to use.</param>
		/// <returns></returns>
		public IEngineBuilder ReadMetadataFrom(IRequestMetadataProvider provider)
		{
			_requestMetadataProvider = provider;
			return this;
		}

		public IEngineBuilder DoSteps(Action<MiddlewareCollection> configureProcessor)
		{
			configureProcessor(_middlewareCollection);

			return this;
		}

		/// <inheritdoc />
		public ILoggerFactory GetLogger()
		{
			return _loggerFactory ?? NullLoggerFactory.Instance;
		}

		private ITransport GetTransport()
		{
			return _transport ?? throw new InvalidOperationException("Transport wasn't configured");
		}

		private IRequestMetadataProvider GetMetadata()
		{
			return _requestMetadataProvider ?? NullRequestMetadataProvider.Instance;
		}

		/// <summary>
		/// Returns constructed instance of the <see cref="IRequestProcessor"/>.
		/// </summary>
		/// <returns></returns>
		public IRequestProcessor BuildProcessor()
		{
			return new RequestProcessor(_middlewareCollection.BuildAll(this));
		}

		/// <summary>
		/// Returns constructed instance of the <see cref="IOrchestrator"/>.
		/// </summary>
		/// <param name="configureOptions">The action for orchestrator options configuration.</param>
		/// <returns></returns>
		public IOrchestrator BuildOrchestrator()
		{
			return new Orchestrator(
				OptionsBuilder.ConfigureAndValidate(_configureOptions),
				GetTransport(),
				GetMetadata(),
				BuildProcessor(),
				GetLogger().CreateLogger<Orchestrator>()
			);
		}
	}
}