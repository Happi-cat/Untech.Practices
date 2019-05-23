using System;
using Untech.AsyncCommandEngine.Builder;

namespace Untech.AsyncCommandEngine.Features.Retrying
{
	public static class EngineBuilderExtensions
	{
		public static PipelineBuilder ThenRetry(this PipelineBuilder builder, IRetryPolicy retryPolicy)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (retryPolicy == null ) throw new ArgumentNullException(nameof(retryPolicy));

			return builder.Then(ctx => new RetryingMiddleware(retryPolicy, ctx.GetLogger()));
		}
	}
}