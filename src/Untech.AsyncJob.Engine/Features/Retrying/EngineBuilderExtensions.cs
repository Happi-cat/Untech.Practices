using System;
using Untech.AsyncJob.Builder;

namespace Untech.AsyncJob.Features.Retrying
{
	public static class EngineBuilderExtensions
	{
		public static PipelineBuilder AddRetry(this PipelineBuilder builder, IRetryPolicy retryPolicy)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (retryPolicy == null) throw new ArgumentNullException(nameof(retryPolicy));

			return builder.Add(ctx => new RetryingMiddleware(retryPolicy, ctx.GetLogger()));
		}
	}
}
