using System;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Features.CQRS;
using Untech.AsyncJob.Formatting;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncJob.Features.SimpleInjector
{
	public static class BuilderExtensions
	{
		public static PipelineBuilder ThenSimpleInjector(this PipelineBuilder collection, Container container)
		{
			return collection.Then(async (ctx, next) =>
			{
				if (ctx.Items.ContainsKey(typeof(Scope)))
				{
					await next(ctx);
				}
				else
				{
					using (var scope = AsyncScopedLifestyle.BeginScope(container))
					{
						ctx.Items[typeof(Scope)] = scope;
						await next(ctx);
					}
				}
			});
		}

		public static void FinalCqrsWithSimpleInjector(this PipelineBuilder collection,
			Container container,
			IRequestTypeFinder requestFinder)
		{
			collection.ThenSimpleInjector(container);
			collection.Final(new CqrsStrategy(requestFinder));
		}

		private class CqrsStrategy : ICqrsStrategy
		{
			private readonly IRequestTypeFinder _requestFinder;

			public CqrsStrategy(IRequestTypeFinder requestFinder)
			{
				_requestFinder = requestFinder;
			}

			public Type FindRequestType(string requestName)
			{
				return _requestFinder.FindRequestType(requestName);
			}

			public IDispatcher GetDispatcher(Context context)
			{
				return GetContentItem<Scope>(context).GetInstance<IDispatcher>();
			}

			public IRequestContentFormatter GetRequestFormatter(Context context)
			{
				return GetContentItem<IRequestContentFormatter>(context);
			}

			private T GetContentItem<T>(Context context)
			{
				return (T)context.Items[typeof(T)];
			}
		}
	}
}
