using System;

namespace Untech.AsyncCommandEngine.Features.Throttling
{
	public static class EngineBuilderExtensions
	{
		public static EngineBuilder UseThrottling(this EngineBuilder builder)
		{
			return UseThrottling(builder, new ThrottleOptions());
		}

		public static EngineBuilder UseThrottling(this EngineBuilder builder, ThrottleOptions options)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (options == null) throw new ArgumentNullException(nameof(options));

			return builder.Use(() => new ThrottleMiddleware(options));
		}
	}
}