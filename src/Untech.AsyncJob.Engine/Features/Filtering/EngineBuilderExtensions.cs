using System;
using Untech.AsyncJob.Builder;

namespace Untech.AsyncJob.Features.Filtering
{
	public static class EngineBuilderExtensions
	{
		public static PipelineBuilder AddFilter(this PipelineBuilder builder, Func<Request, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			return builder.Add(ctx => new FilteringMiddleware(ctx.GetLogger(), predicate));
		}
	}
}