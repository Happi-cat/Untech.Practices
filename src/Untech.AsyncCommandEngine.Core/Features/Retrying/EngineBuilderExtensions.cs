using System;

namespace Untech.AsyncCommandEngine.Features.Retrying
{
	public static class EngineBuilderExtensions
	{
		public static MiddlewareCollection ThenRetry(this MiddlewareCollection builder, IRetryPolicy retryPolicy)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (retryPolicy == null ) throw new ArgumentNullException(nameof(retryPolicy));

			return builder.Then(ctx => new RetryingMiddleware(retryPolicy, ctx.GetLogger()));
		}
	}
}