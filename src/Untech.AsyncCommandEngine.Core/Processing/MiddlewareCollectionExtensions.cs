using System;
using Untech.AsyncCommandEngine.Builder;

namespace Untech.AsyncCommandEngine.Processing
{
	public static class MiddlewareCollectionExtensions
	{
		public static void Final(this MiddlewareCollection collection, ICqrsStrategy strategy)
		{
			collection.Then(ctx => new CqrsMiddleware(strategy));
		}

		public static void Final(this MiddlewareCollection collection, Func<IBuilderContext, ICqrsStrategy> strategy)
		{
			collection.Then(ctx => new CqrsMiddleware(strategy(ctx)));
		}
	}
}