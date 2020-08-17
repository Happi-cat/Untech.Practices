using System;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.SimpleInjector
{
	public static class BuilderExtensions
	{
		public static IRegistrar<IRequestProcessorMiddleware> AddSimpleInjector(this IRegistrar<IRequestProcessorMiddleware> collection, Container container)
		{
			return collection.Add(async (ctx, next) =>
			{
				if (IsServiceProviderAttached(ctx))
				{
					await next(ctx);
				}
				else
				{
					using (var scope = AsyncScopedLifestyle.BeginScope(container))
					{
						AttachServiceProvider(ctx, new ScopeAdapter(scope));
						await next(ctx);
					}
				}
			});
		}

		private static bool IsServiceProviderAttached(Context context)
		{
			return context.Items.ContainsKey(typeof(IServiceProvider));
		}

		private static void AttachServiceProvider(Context context, IServiceProvider serviceProvider)
		{
			context.Items[typeof(IServiceProvider)] = serviceProvider;
		}

		private class ScopeAdapter : IServiceProvider
		{
			private readonly Scope _scope;

			public ScopeAdapter(Scope scope)
			{
				_scope = scope;
			}

			public object GetService(Type serviceType)
			{
				return _scope.GetInstance(serviceType);
			}
		}
	}
}
