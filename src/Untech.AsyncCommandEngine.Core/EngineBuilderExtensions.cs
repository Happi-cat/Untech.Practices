using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;
using Untech.AsyncCommandEngine.Transports;

namespace Untech.AsyncCommandEngine
{
	/// <summary>
	/// Contains set of methods that can be used during work with <see cref="EngineBuilder"/>.
	/// </summary>
	public static class EngineBuilderExtensions
	{
		/// <summary>
		/// Gets instance of the <see cref="ILogger{TCategoryName}"/>
		/// </summary>
		/// <param name="builder"></param>
		/// <typeparam name="T">The type to use as category.</typeparam>
		/// <returns></returns>
		public static ILogger<T> GetLogger<T>(this IEngineBuilderContext builder)
		{
			return builder.GetLogger().CreateLogger<T>();
		}

		/// <summary>
		/// Gets instance of the <see cref="ILogger"/> for the specified <paramref name="categoryName"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="categoryName">The name of category.</param>
		/// <returns></returns>
		public static ILogger GetLogger(this IEngineBuilderContext builder, string categoryName)
		{
			return builder.GetLogger().CreateLogger(categoryName);
		}

		public static EngineBuilder ReceiveRequestsFrom(this EngineBuilder builder,
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
		public static EngineBuilder ReceiveRequestsFrom(this EngineBuilder builder,
			IEnumerable<ITransport> transports)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (transports == null) throw new ArgumentNullException(nameof(transports));

			return builder.ReceiveRequestsFrom(new CompositeTransport(transports));
		}

		public static EngineBuilder ReadMetadataFrom(this EngineBuilder builder,
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
		public static EngineBuilder ReadMetadataFrom(this EngineBuilder builder,
			IEnumerable<IRequestMetadataProvider> providers)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (providers == null) throw new ArgumentNullException(nameof(providers));

			return builder.ReadMetadataFrom(new CompositeRequestMetadataProvider(providers));
		}

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="builder">The builder to use for middleware registration.</param>
		/// <param name="middleware">The instance of the <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public static MiddlewareCollection Then(this MiddlewareCollection builder,
			IRequestProcessorMiddleware middleware)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (middleware == null) throw new ArgumentNullException(nameof(middleware));

			return builder.Then(ctx => middleware);
		}

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="builder">The builder to use for middleware registration.</param>
		/// <param name="middleware">The <see cref="IRequestProcessor"/> middleware.</param>
		/// <returns></returns>
		public static MiddlewareCollection Then(this MiddlewareCollection builder,
			Func<Context, RequestProcessorCallback, Task> middleware)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (middleware == null) throw new ArgumentNullException(nameof(middleware));

			return builder.Then(ctx => new AdHocRequestProcessorMiddleware(middleware));
		}

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="builder">The builder to use for middleware registration.</param>
		/// <param name="creator">The function that creates <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public static MiddlewareCollection Then(this MiddlewareCollection builder,
			Func<IEngineBuilderContext, Func<Context, RequestProcessorCallback, Task>> creator)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (creator == null) throw new ArgumentNullException(nameof(creator));

			return builder.Then(ctx => new AdHocRequestProcessorMiddleware(creator(ctx)));
		}
	}
}