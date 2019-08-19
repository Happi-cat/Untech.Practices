using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Processing;
using Untech.AsyncJob.Transports;

namespace Untech.AsyncJob.Builder
{
	/// <summary>
	/// Used for <see cref="IOrchestrator"/> and <see cref="IRequestProcessor"/> configuration.
	/// </summary>
	public class EngineBuilder : IBuilderContext, IEngineBuilder
	{
		private readonly Action<OrchestratorOptions> _configureOptions;
		private readonly PipelineBuilder _pipelineBuilder;

		private Func<ILoggerFactory> _loggerCreator;
		private ILoggerFactory _logger;

		private Func<IBuilderContext, ITransport> _transportCreator;
		private ITransport _transport;

		private Func<IBuilderContext, IRequestMetadataProvider> _providerCreator;
		private IRequestMetadataProvider _provider;

		public EngineBuilder()
			: this(_ => { })
		{

		}

		public EngineBuilder(Action<OrchestratorOptions> configureOptions)
		{
			_pipelineBuilder = new PipelineBuilder();
			_configureOptions = configureOptions;
		}

		/// <summary>
		/// Sets <see cref="ILoggerFactory"/> that will be used for logging.
		/// </summary>
		/// <param name="loggerCreator">The logger factory.</param>
		/// <returns></returns>
		public IEngineBuilder LogTo(Func<ILoggerFactory> loggerCreator)
		{
			_loggerCreator = loggerCreator;
			return this;
		}

		/// <summary>
		/// Sets <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="transportCreator">The transport to use.</param>
		/// <returns></returns>
		public IEngineBuilder ReceiveRequestsFrom(Func<IBuilderContext, ITransport> transportCreator)
		{
			_transportCreator = transportCreator;
			return this;
		}

		/// <summary>
		/// Sets <see cref="IRequestMetadataProvider"/> that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="providerCreator">The provider to use.</param>
		/// <returns></returns>
		public IEngineBuilder ReadMetadataFrom(Func<IBuilderContext, IRequestMetadataProvider> providerCreator)
		{
			_providerCreator = providerCreator;
			return this;
		}

		public IEngineBuilder Do(Action<PipelineBuilder> configureProcessor)
		{
			configureProcessor(_pipelineBuilder);

			return this;
		}

		/// <inheritdoc />
		public ILoggerFactory GetLogger()
		{
			if (_logger != null)
				return _logger;
			return _logger = _loggerCreator?.Invoke() ?? NullLoggerFactory.Instance;
		}

		private ITransport GetTransport()
		{
			if (_transport != null)
				return _transport;
			return _transport = _transportCreator?.Invoke(this) ?? throw NotConfigured();

			Exception NotConfigured()
			{
				return new InvalidOperationException("Transport wasn't configured");
			}
		}

		private IRequestMetadataProvider GetMetadata()
		{
			if (_provider != null)
				return _provider;
			return _provider = _providerCreator?.Invoke(this) ?? NullRequestMetadataProvider.Instance;
		}

		/// <summary>
		/// Returns constructed instance of the <see cref="IRequestProcessor"/>.
		/// </summary>
		/// <returns></returns>
		public IRequestProcessor BuildProcessor()
		{
			return new RequestProcessor(_pipelineBuilder.BuildAll(this));
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
