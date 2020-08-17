using System;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.Filtering
{
	public static class EngineBuilderExtensions
	{
		public static IRegistrar<IRequestProcessorMiddleware> AddFilter(this IRegistrar<IRequestProcessorMiddleware> builder, Func<Request, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			return builder.Add(ctx => new FilteringMiddleware(ctx.GetLogger(), predicate));
		}
	}
}