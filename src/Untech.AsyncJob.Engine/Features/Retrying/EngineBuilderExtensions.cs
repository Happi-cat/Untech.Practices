using System;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.Retrying
{
	public static class EngineBuilderExtensions
	{
		public static IRegistrar<IRequestProcessorMiddleware> AddRetry(this IRegistrar<IRequestProcessorMiddleware> builder, IRetryPolicy retryPolicy)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (retryPolicy == null) throw new ArgumentNullException(nameof(retryPolicy));

			return builder.Add(ctx => new RetryingMiddleware(retryPolicy, ctx.GetLogger()));
		}
	}
}
