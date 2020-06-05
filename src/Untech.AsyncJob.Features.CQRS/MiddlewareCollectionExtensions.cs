using System;
using Untech.AsyncJob.Builder;

namespace Untech.AsyncJob.Features.CQRS
{
	public static class MiddlewareCollectionExtensions
	{
		public static void Final(this PipelineBuilder collection, ICqrsStrategy strategy)
		{
			collection.Add(ctx => new CqrsMiddleware(strategy));
		}

		public static void Final(this PipelineBuilder collection, Func<IServiceProvider, ICqrsStrategy> strategy)
		{
			collection.Add(ctx => new CqrsMiddleware(strategy(ctx)));
		}
	}
}
