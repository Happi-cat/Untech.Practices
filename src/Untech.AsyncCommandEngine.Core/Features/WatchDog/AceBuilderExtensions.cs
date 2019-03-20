using System;

namespace Untech.AsyncCommandEngine.Features.WatchDog
{
	public static class AceBuilderExtensions
	{
		public static AceBuilder UseWatchDog(this AceBuilder builder)
		{
			return UseWatchDog(builder, new WatchDogOptions());
		}

		public static AceBuilder UseWatchDog(this AceBuilder builder, WatchDogOptions options)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (options == null) throw new ArgumentNullException(nameof(options));

			return builder.Use(() => new WatchDogMiddleware(options));
		}
	}
}