using System;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Untech.AsyncCommandEngine.Builder;
using Untech.AsyncCommandEngine.Processing;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncCommandEngine.Features.SimpleInjector
{
	public static class BuilderExtensions
	{
		public static PipelineBuilder ThenSimpleInjector(this PipelineBuilder collection, Container container)
		{
			return collection.Then(async (ctx, next) =>
			{
				using (var scope = AsyncScopedLifestyle.BeginScope(container))
				{
					ctx.Items[typeof(Scope)] = scope;
					await next(ctx);
				}
			});
		}

		public static void FinalSimpleInjector(this PipelineBuilder collection, Func<string, Type> requestFinder)
		{
			collection.Final(new CqrsStrategy(requestFinder));
		}

		private class CqrsStrategy : ICqrsStrategy
		{
			private readonly Func<string, Type> _requestFinder;

			public CqrsStrategy(Func<string, Type> requestFinder)
			{
				_requestFinder = requestFinder;
			}

			public Type FindRequestType(string requestName)
			{
				return _requestFinder(requestName);
			}

			public IDispatcher GetDispatcher(Context context)
			{
				return ((Scope)context.Items[typeof(Scope)]).GetInstance<IDispatcher>();
			}
		}
	}
}