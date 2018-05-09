using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal class CommandHandlerInvoker<TIn, TOut> : RequestHandlerInvoker<TIn, TOut>
		where TIn : ICommand<TOut>
	{
		private readonly ITypeResolver _resolver;

		public CommandHandlerInvoker(ITypeResolver resolver, IHandlerInitializer handlerInitializer) : base(resolver, handlerInitializer)
		{
			_resolver = resolver;
		}

		public override object Invoke(object args)
		{
			var syncHandler = _resolver.ResolveOne<ICommandHandler<TIn, TOut>>();
			if (syncHandler != null)
			{
				return Invoke(syncHandler, (TIn)args);
			}

			var asyncHandler = _resolver.ResolveOne<ICommandAsyncHandler<TIn, TOut>>();
			if (asyncHandler != null)
			{
				return InvokeAsync(asyncHandler, (TIn)args, CancellationToken.None).Result;
			}

			throw new InvalidOperationException("Handler wasn't implemented");
		}

		public override Task InvokeAsync(object args, CancellationToken cancellationToken)
		{
			var asyncHandler = _resolver.ResolveOne<ICommandAsyncHandler<TIn, TOut>>();
			if (asyncHandler != null)
			{
				return InvokeAsync(asyncHandler, (TIn)args, cancellationToken);
			}

			var syncHandler = _resolver.ResolveOne<ICommandHandler<TIn, TOut>>();
			if (syncHandler != null)
			{
				return Task.FromResult(Invoke(syncHandler, (TIn)args));
			}

			throw new InvalidOperationException("Handler wasn't implemented");
		}
	}
}