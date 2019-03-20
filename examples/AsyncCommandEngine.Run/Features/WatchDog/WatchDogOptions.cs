using System;
using System.Collections.ObjectModel;

namespace AsyncCommandEngine.Run
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

			return builder.Use(() => new WatchDogProcessorMiddleware(options));
		}
	}

	public class WatchDogOptions
	{
		/// <summary>
		/// Null is for disabled default timeout
		/// </summary>
		public TimeSpan? DefaultTimeout { get; set; }

		public ReadOnlyDictionary<string, TimeSpan> RequestTimeouts { get; set; }
	}
}