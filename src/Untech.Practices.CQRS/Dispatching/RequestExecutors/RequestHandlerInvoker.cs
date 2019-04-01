using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;
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

		public Task InvokeAsync(object args, CancellationToken cancellationToken)
		{
			return new RequestHandler(_resolver)
				.HandleAsync((TIn)args, cancellationToken);
		}

		private static InvalidOperationException CreateHandlerNotFoundException()
		{
			return new InvalidOperationException($"Handler wasn't found for {typeof(TIn)} request.");
		}

		private static void PreProcess(ITypeResolver typeResolver, TIn request)
		{
			var preProcessors = typeResolver.ResolveMany<IPipelinePreProcessor<TIn>>();
			foreach (IPipelinePreProcessor<TIn> preProcessor in preProcessors)
				preProcessor.Process(request);
		}

		private static void PostProcess(ITypeResolver typeResolver, TIn request, TOut result)
		{
			var postProcessors = typeResolver.ResolveMany<IPipelinePostProcessor<TIn, TOut>>();
			foreach (IPipelinePostProcessor<TIn, TOut> postProcessor in postProcessors)
				postProcessor.Process(request, result);
		}

		private class RequestHandler : IRequestHandler<TIn, TOut>
		{
			private readonly ITypeResolver _resolver;

			public RequestHandler(ITypeResolver resolver)
			{
				_resolver = resolver;
			}

			public async Task<TOut> HandleAsync(TIn request, CancellationToken cancellationToken)
			{
				IRequestHandler<TIn, TOut> handler = GetAsyncHandlerOrThrow();

				PreProcess(_resolver, request);

				TOut result = await handler.HandleAsync(request, cancellationToken);

				PostProcess(_resolver, request, result);

				return result;
			}

			private IRequestHandler<TIn, TOut> GetAsyncHandlerOrThrow()
			{
				IRequestHandler<TIn, TOut> handler = _resolver.ResolveOne<IRequestHandler<TIn, TOut>>();
				if (handler != null)
					return handler;

				throw CreateHandlerNotFoundException();
			}
		}
	}
}