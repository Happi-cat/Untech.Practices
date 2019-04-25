using System;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine
{
	public static class EngineBuilderExtensions
	{
		public static EngineBuilder Use(this EngineBuilder builder,
			IRequestProcessorMiddleware middleware)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (middleware == null) throw new ArgumentNullException(nameof(middleware));

			return builder.Use(ctx => middleware);
		}

		public static EngineBuilder Use(this EngineBuilder builder,
			Func<Context, RequestProcessorCallback, Task> middleware)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (middleware == null) throw new ArgumentNullException(nameof(middleware));

			return builder.Use(ctx => new AdHocRequestProcessorMiddleware(middleware));
		}

		public static EngineBuilder Use(this EngineBuilder builder,
			Func<IEngineBuilderContext, Func<Context, RequestProcessorCallback, Task>> creator)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (creator == null) throw new ArgumentNullException(nameof(creator));

			return builder.Use(ctx => new AdHocRequestProcessorMiddleware(creator(ctx)));
		}
	}
}