using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Transports;

namespace Untech.AsyncCommandEngine.Builder
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
			return builder.ReceiveRequestsFrom(transports.AsEnumerable());
		}

		/// <summary>
		/// Sets a collection of <see cref="ITransport"/> that will be used as a request store.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="transports">The collection of transport to use.</param>
		/// <returns></returns>
		public static IEngineBuilder ReceiveRequestsFrom(this IEngineBuilder builder,
			IEnumerable<ITransport> transports)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (transports == null) throw new ArgumentNullException(nameof(transports));

			return builder.ReceiveRequestsFrom(new CompositeTransport(transports));
		}

		/// <summary>
		/// Sets a collection of <see cref="IRequestMetadataProvider"/> that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="providers">The collection of providers to use.</param>
		/// <returns></returns>
		public static IEngineBuilder ReadMetadataFrom(this IEngineBuilder builder,
			params IRequestMetadataProvider[] providers)
		{
			return builder.ReadMetadataFrom(providers.AsEnumerable());
		}

		/// <summary>
		/// Sets a collection of <see cref="IRequestMetadataProvider"/> that can be used for getting <see cref="IRequestMetadata"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="providers">The collection of providers to use.</param>
		/// <returns></returns>
		public static IEngineBuilder ReadMetadataFrom(this IEngineBuilder builder,
			IEnumerable<IRequestMetadataProvider> providers)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (providers == null) throw new ArgumentNullException(nameof(providers));

			return builder.ReadMetadataFrom(new CompositeRequestMetadataProvider(providers));
		}
	}
}