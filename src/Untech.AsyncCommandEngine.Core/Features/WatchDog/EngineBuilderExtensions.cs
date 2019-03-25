using System;

namespace Untech.AsyncCommandEngine.Features.WatchDog
{
	public static class EngineBuilderExtensions
	{
		public static EngineBuilder UseWatchDog(this EngineBuilder builder)
		{
			return UseWatchDog(builder, new WatchDogOptions());
		}

		public static EngineBuilder UseWatchDog(this EngineBuilder builder, WatchDogOptions options)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (options == null) throw new ArgumentNullException(nameof(options));

			return builder.Use(() => new WatchDogMiddleware(options));
		}
	}
}