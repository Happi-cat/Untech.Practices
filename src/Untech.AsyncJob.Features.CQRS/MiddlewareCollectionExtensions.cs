using System;
using System.Collections;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.CQRS
{
	public static class MiddlewareCollectionExtensions
	{
		public static IEngineBuilder Finally(this IEngineBuilder collection, ICqrsStrategy strategy)
		{
			return collection.Finally(r => r
				.Add(new CqrsProcessor(strategy))
			);
		}

		public static IEngineBuilder Finally(this IEngineBuilder collection, Func<IServiceProvider, ICqrsStrategy> strategy)
		{
			return collection.Finally(r => r
				.Add(ctx => new CqrsProcessor(strategy(ctx)))
			);
		}
	}
}
