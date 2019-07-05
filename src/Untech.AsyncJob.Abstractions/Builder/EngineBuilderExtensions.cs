using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Transports;

namespace Untech.AsyncJob.Builder
{
	/// <summary>
	/// Contains set of methods that can be used during work with <see cref="IEngineBuilder"/>.
	/// </summary>
	public static class EngineBuilderExtensions
	{
		/// <summary>
		/// Gets instance of the <see cref="ILogger{TCategoryName}"/>
		/// </summary>
		/// <param name="builder"></param>
		/// <typeparam name="T">The type to use as category.</typeparam>
		/// <returns></returns>
		public static ILogger<T> GetLogger<T>(this IBuilderContext builder)
		{
			return builder.GetLogger().CreateLogger<T>();
		}

		/// <summary>
		/// Gets instance of the <see cref="ILogger"/> for the specified <paramref name="categoryName"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="categoryName">The name of category.</param>
		/// <returns></returns>
		public static ILogger GetLogger(this IBuilderContext builder, string categoryName)
		{
			return builder.GetLogger().CreateLogger(categoryName);
		}

		/// <summary>
		/// Sets a collection of <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="transports">The collection of transport to use.</param>
		/// <returns></returns>
		public static IEngineBuilder ReceiveRequestsFrom(this IEngineBuilder builder,
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
			Func<IBuilderContext, IEnumerable<ITransport>> transportsCreator)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (transportsCreator == null) throw new ArgumentNullException(nameof(transportsCreator));

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
			Func<IBuilderContext, IEnumerable<IRequestMetadataProvider>> providersCreator)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (providersCreator == null) throw new ArgumentNullException(nameof(providersCreator));

			return builder.ReadMetadataFrom(ctx => new CompositeRequestMetadataProvider(providersCreator(ctx)));
		}
	}
}
