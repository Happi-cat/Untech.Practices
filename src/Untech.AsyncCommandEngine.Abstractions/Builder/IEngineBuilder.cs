using System;
using Microsoft.Extensions.Logging;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;
using Untech.AsyncCommandEngine.Transports;

namespace Untech.AsyncCommandEngine.Builder
{
	public interface IEngineBuilder
	{
		/// <summary>
		/// Sets <see cref="Microsoft.Extensions.Logging.ILoggerFactory"/> that will be used for logging.
		/// </summary>
		/// <param name="loggerFactory">The logger factory.</param>
		/// <returns></returns>
		IEngineBuilder LogTo(ILoggerFactory loggerFactory);

		/// <summary>
		/// Sets <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="transport">The transport to use.</param>
		/// <returns></returns>
		IEngineBuilder ReceiveRequestsFrom(ITransport transport);

		/// <summary>
		/// Sets <see cref="IRequestMetadataProvider"/> that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="provider">The provider to use.</param>
		/// <returns></returns>
		IEngineBuilder ReadMetadataFrom(IRequestMetadataProvider provider);

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