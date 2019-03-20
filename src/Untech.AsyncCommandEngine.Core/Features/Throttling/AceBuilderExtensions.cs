using System;

namespace Untech.AsyncCommandEngine.Features.Throttling
{
	public static class AceBuilderExtensions
	{
		public static AceBuilder UseThrottling(this AceBuilder builder)
		{
			return UseThrottling(builder, new ThrottleOptions());
		}

		public static AceBuilder UseThrottling(this AceBuilder builder, ThrottleOptions options)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (options == null) throw new ArgumentNullException(nameof(options));

			return builder.Use(() => new ThrottleMiddleware(options));
		}
	}
}