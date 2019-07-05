using System;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Processing;
using Untech.AsyncJob.Transports;

namespace Untech.AsyncJob.Builder
{
	public interface IEngineBuilder
	{
		/// <summary>
		/// Sets <see cref="Microsoft.Extensions.Logging.ILoggerFactory"/> that will be used for logging.
		/// </summary>
		/// <param name="loggerCreator">The logger factory.</param>
		/// <returns></returns>
		IEngineBuilder LogTo(Func<ILoggerFactory> loggerCreator);

		/// <summary>
		/// Sets <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="transportCreator">The transport to use.</param>
		/// <returns></returns>
		IEngineBuilder ReceiveRequestsFrom(Func<IBuilderContext, ITransport> transportCreator);

		/// <summary>
		/// Sets <see cref="IRequestMetadataProvider"/> that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="providerCreator">The provider to use.</param>
		/// <returns></returns>
		IEngineBuilder ReadMetadataFrom(Func<IBuilderContext, IRequestMetadataProvider> providerCreator);

		IEngineBuilder Do(Action<PipelineBuilder> configureProcessor);

		/// <summary>
		/// Returns constructed instance of the <see cref="IRequestProcessor"/>.
		/// </summary>
		/// <returns></returns>
		IRequestProcessor BuildProcessor();

		/// <summary>
		/// Returns constructed instance of the <see cref="IOrchestrator"/>.
		/// </summary>
		/// <param name="configureOptions">The action for orchestrator options configuration.</param>
		/// <returns></returns>
		IOrchestrator BuildOrchestrator();
	}
}
