using System;

namespace Untech.AsyncCommandEngine.Features.WatchDog
{
	public static class EngineBuilderExtensions
	{
		public static EngineBuilder ThenWatchDog(this EngineBuilder builder)
		{
			return ThenWatchDog(builder, new WatchDogOptions());
		}

		public static EngineBuilder ThenWatchDog(this EngineBuilder builder, WatchDogOptions options)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (options == null) throw new ArgumentNullException(nameof(options));

			return builder.Then(ctx => new WatchDogMiddleware(options, ctx.GetLogger()));
		}
	}
}