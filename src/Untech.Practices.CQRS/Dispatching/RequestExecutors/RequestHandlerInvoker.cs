using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.CQRS.Handlers.Specialized;
using Untech.Practices.CQRS.Pipeline;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal class RequestHandlerInvoker<TIn, TOut> : IHandlerInvoker
		where TIn : IRequest<TOut>
	{
		private readonly ITypeResolver _resolver;

		public RequestHandlerInvoker(ITypeResolver resolver)
		{
			_resolver = resolver;
		}

		public object Invoke(object args)
		{
			return new RunHandler(_resolver)
				.Handle((TIn)args);
		}

		public Task InvokeAsync(object args, CancellationToken cancellationToken)
		{
			return new RunAsyncHandler(_resolver)
				.HandleAsync((TIn)args, cancellationToken);
		}

		private static InvalidOperationException CreateHandlerNotFoundException()
		{
			return new InvalidOperationException($"Handler wasn't found for {typeof(TIn)} request.");
		}

		private static void PreProcess(ITypeResolver typeResolver, TIn request)
		{
			foreach (IPipelinePreProcessor<TIn> preProcessor in ResolveMany<IPipelinePreProcessor<TIn>>(typeResolver))
				preProcessor.Process(request);
		}

		private static void PostProcess(ITypeResolver typeResolver, TIn request, TOut result)
		{
			foreach (IPipelinePostProcessor<TIn, TOut> postProcessor in
				ResolveMany<IPipelinePostProcessor<TIn, TOut>>(typeResolver)) postProcessor.Process(request, result);
		}

		private static IEnumerable<T> ResolveMany<T>(ITypeResolver resolver)
			where T : class
		{
			return (resolver.ResolveMany<T>() ?? Enumerable.Empty<T>())
				.Where(n => n != null);
		}

		private class RunHandler : IRequestHandler<TIn, TOut>
		{
			private readonly ITypeResolver _resolver;

			public RunHandler(ITypeResolver resolver)
			{
				_resolver = resolver;
			}

			public TOut Handle(TIn request)
			{
				IRequestHandler<TIn, TOut> handler = GetHandlerOrThrow();

				PreProcess(_resolver, request);

				TOut result = handler.Handle(request);

				PostProcess(_resolver, request, result);

				return result;
			}

			private IRequestHandler<TIn, TOut> GetHandlerOrThrow()
			{
				IRequestHandler<TIn, TOut> syncHandler = _resolver.ResolveOne<IRequestHandler<TIn, TOut>>();
				if (syncHandler != null)
					return syncHandler;

				IRequestAsyncHandler<TIn, TOut> asyncHandler = _resolver.ResolveOne<IRequestAsyncHandler<TIn, TOut>>();
				if (asyncHandler != null)
					return new AsyncAsSyncRequestHandler<TIn, TOut>(asyncHandler);

				throw CreateHandlerNotFoundException();
			}
		}

		private class RunAsyncHandler : IRequestAsyncHandler<TIn, TOut>
		{
			private readonly ITypeResolver _resolver;

			public RunAsyncHandler(ITypeResolver resolver)
			{
				_resolver = resolver;
			}

			public async Task<TOut> HandleAsync(TIn request, CancellationToken cancellationToken)
			{
				IRequestAsyncHandler<TIn, TOut> handler = GetAsyncHandlerOrThrow();

				PreProcess(_resolver, request);

				TOut result = await handler.HandleAsync(request, cancellationToken);

				PostProcess(_resolver, request, result);

				return result;
			}

			private IRequestAsyncHandler<TIn, TOut> GetAsyncHandlerOrThrow()
			{
				IRequestAsyncHandler<TIn, TOut> asyncHandler = _resolver.ResolveOne<IRequestAsyncHandler<TIn, TOut>>();
				if (asyncHandler != null)
					return asyncHandler;

				IRequestHandler<TIn, TOut> syncHandler = _resolver.ResolveOne<IRequestHandler<TIn, TOut>>();
				if (syncHandler != null)
					return new SyncAsAsyncRequestHandler<TIn, TOut>(syncHandler);

				throw CreateHandlerNotFoundException();
			}
		}
	}
}