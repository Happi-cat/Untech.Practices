using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;
using Untech.AsyncCommandEngine.Transports;

namespace Untech.AsyncCommandEngine
{
	public static class EngineBuilderExtensions
	{
		public static EngineBuilder ReceiveRequestsFrom(this EngineBuilder builder,
			IEnumerable<ITransport> transports)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (transports == null) throw new ArgumentNullException(nameof(transports));

			return builder.ReceiveRequestsFrom(new CompositeTransport(transports));
		}

		public static EngineBuilder ReadMetadataFrom(this EngineBuilder builder,
			IEnumerable<IRequestMetadataProvider> metadata)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (metadata == null) throw new ArgumentNullException(nameof(metadata));

			return builder.ReadMetadataFrom(new CompositeRequestMetadataProvider(metadata));
		}

		public static EngineBuilder Then(this EngineBuilder builder,
			IRequestProcessorMiddleware middleware)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (middleware == null) throw new ArgumentNullException(nameof(middleware));

			return builder.Then(ctx => middleware);
		}

		public static EngineBuilder Then(this EngineBuilder builder,
			Func<Context, RequestProcessorCallback, Task> middleware)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (middleware == null) throw new ArgumentNullException(nameof(middleware));

			return builder.Then(ctx => new AdHocRequestProcessorMiddleware(middleware));
		}

		public static EngineBuilder Then(this EngineBuilder builder,
			Func<IEngineBuilderContext, Func<Context, RequestProcessorCallback, Task>> creator)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (creator == null) throw new ArgumentNullException(nameof(creator));

			return builder.Then(ctx => new AdHocRequestProcessorMiddleware(creator(ctx)));
		}
	}
}