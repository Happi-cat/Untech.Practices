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
		/// <param name="factory">The logger factory.</param>
		/// <returns></returns>
		IEngineBuilder LogTo(Func<ILoggerFactory> factory);

		/// <summary>
		/// Sets <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="configure">The transport to use.</param>
		/// <returns></returns>
		IEngineBuilder ReceiveRequestsFrom(Action<IRegistrar<ITransport>> configure);

		/// <summary>
		/// Sets <see cref="IRequestMetadataProvider"/> that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="configure">Action that configures providers to use.</param>
		/// <returns></returns>
		IEngineBuilder ReadMetadataFrom(Action<IRegistrar<IRequestMetadataProvider>> configure);

		/// <summary>
		/// Sets <see cref="IRequestProcessorMiddleware"/> that can be used for preprocessing.
		/// </summary>
		/// <param name="configure">Action that configures middlewares.</param>
		/// <returns></returns>
		IEngineBuilder Then(Action<IRegistrar<IRequestProcessorMiddleware>> configure);

		/// <summary>
		/// Sets <see cref="IRequestProcessor"/> final request processor.
		/// </summary>
		/// <param name="configure">Action that configures processor.</param>
		/// <returns></returns>
		IEngineBuilder Finally(Action<IRegistrar<IRequestProcessor>> configure);

		/// <summary>
		/// Returns constructed instance of the <see cref="IRequestProcessor"/>.
		/// </summary>
		/// <returns></returns>
		IRequestProcessor BuildProcessor();

		/// <summary>
		/// Returns constructed instance of the <see cref="IOrchestrator"/>.
		/// </summary>
		/// <returns></returns>
		IOrchestrator BuildOrchestrator();
	}
}
