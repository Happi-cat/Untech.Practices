using System;
using System.Collections.Generic;
using System.Linq;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Transports;

namespace Untech.AsyncJob.Builder
{
	public static class EngineBuilderExtensions
	{
		/// <summary>
		/// Sets a collection of <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="transports">The collection of transport to use.</param>
		/// <returns></returns>
		public static IEngineBuilder ReceiveRequestsFromMultiple(this IEngineBuilder builder,
			params ITransport[] transports)
		{
			return builder.ReceiveRequestsFromMultiple(ctx => transports.AsEnumerable());
		}

		/// <summary>
		/// Sets a collection of <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="transportsCreator">The collection of transport to use.</param>
		/// <returns></returns>
		public static IEngineBuilder ReceiveRequestsFromMultiple(this IEngineBuilder builder,
			Func<IServiceProvider, IEnumerable<ITransport>> transportsCreator)
		{
			if (builder == null)
				throw new ArgumentNullException(nameof(builder));
			if (transportsCreator == null)
				throw new ArgumentNullException(nameof(transportsCreator));

			return builder.ReceiveRequestsFrom(ctx => new CompositeTransport(transportsCreator(ctx)));
		}

		/// <summary>
		/// Sets a collection of <see cref="IRequestMetadataProvider"/>
		/// that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="providers">The collection of providers to use.</param>
		/// <returns></returns>
		public static IEngineBuilder ReadMetadataFromMultiple(this IEngineBuilder builder,
			params IRequestMetadataProvider[] providers)
		{
			return builder.ReadMetadataFromMultiple(ctx => providers.AsEnumerable());
		}

		/// <summary>
		/// Sets a collection of <see cref="IRequestMetadataProvider"/>
		/// that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="providersCreator">The collection of providers to use.</param>
		/// <returns></returns>
		public static IEngineBuilder ReadMetadataFromMultiple(this IEngineBuilder builder,
			Func<IServiceProvider, IEnumerable<IRequestMetadataProvider>> providersCreator)
		{
			if (builder == null)
				throw new ArgumentNullException(nameof(builder));
			if (providersCreator == null)
				throw new ArgumentNullException(nameof(providersCreator));

			return builder.ReadMetadataFrom(ctx => new CompositeRequestMetadataProvider(providersCreator(ctx)));
		}
	}
}
