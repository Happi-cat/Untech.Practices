using System;

namespace Untech.AsyncCommandEngine.Features.Throttling
{
	public static class EngineBuilderExtensions
	{
		public static EngineBuilder ThenThrottling(this EngineBuilder builder)
		{
			return ThenThrottling(builder, new ThrottleOptions());
		}

		public static EngineBuilder ThenThrottling(this EngineBuilder builder, ThrottleOptions options)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (options == null) throw new ArgumentNullException(nameof(options));

			return builder.Then(ctx => new ThrottleMiddleware(options));
		}
	}
}