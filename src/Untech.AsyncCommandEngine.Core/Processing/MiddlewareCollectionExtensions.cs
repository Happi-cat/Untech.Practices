using System;
using Untech.AsyncCommandEngine.Builder;

namespace Untech.AsyncCommandEngine.Processing
{
	public static class MiddlewareCollectionExtensions
	{
		public static void Final(this PipelineBuilder collection, ICqrsStrategy strategy)
		{
			collection.Then(ctx => new CqrsMiddleware(strategy));
		}

		public static void Final(this PipelineBuilder collection, Func<IBuilderContext, ICqrsStrategy> strategy)
		{
			collection.Then(ctx => new CqrsMiddleware(strategy(ctx)));
		}
	}
}