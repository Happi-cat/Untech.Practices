using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal class QueryHandlerRunner<TIn, TOut> : RequestHandlerRunner<TIn, TOut>
		where TIn : IQuery<TOut>
	{
		private readonly ITypeResolver _resolver;

		public QueryHandlerRunner(ITypeResolver resolver, ITypeInitializer typeInitializer) : base(resolver, typeInitializer)
		{
			_resolver = resolver;
		}

		public override object Handle(object args)
		{
			var syncHandler = _resolver.ResolveOne<IQueryHandler<TIn, TOut>>();
			if (syncHandler != null)
			{
				return Handle(syncHandler, (TIn)args);
			}

			var asyncHandler = _resolver.ResolveOne<IQueryAsyncHandler<TIn, TOut>>();
			if (asyncHandler != null)
			{
				return HandleAsync(asyncHandler, (TIn)args, CancellationToken.None).Result;
			}

			throw new InvalidOperationException("Handler wasn't implemented");
		}

		public override Task HandleAsync(object args, CancellationToken cancellationToken)
		{
			var asyncHandler = _resolver.ResolveOne<IQueryAsyncHandler<TIn, TOut>>();
			if (asyncHandler != null)
			{
				return HandleAsync(asyncHandler, (TIn)args, cancellationToken);
			}

			var syncHandler = _resolver.ResolveOne<IQueryHandler<TIn, TOut>>();
			if (syncHandler != null)
			{
				return Task.FromResult(Handle(syncHandler, (TIn)args));
			}

			throw new InvalidOperationException("Handler wasn't implemented");
		}
	}
}