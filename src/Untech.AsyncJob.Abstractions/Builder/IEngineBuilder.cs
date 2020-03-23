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
		/// <param name="configure">The provider to use.</param>
		/// <returns></returns>
		IEngineBuilder ReadMetadataFrom(Action<IRegistrar<IRequestMetadataProvider>> configure);

		IEngineBuilder Then(Action<IRegistrar<IRequestProcessorMiddleware>> configure);

		IEngineBuilder Finally(Action<IRegistrar<IRequestProcessor>> configure);

		IRequestProcessor BuildProcessor();

		/// <summary>
		/// Returns constructed instance of the <see cref="IOrchestrator"/>.
		/// </summary>
		/// <returns></returns>
		IOrchestrator BuildOrchestrator();
	}
}
