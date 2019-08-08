using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.CQRS.Pipeline;

namespace Untech.Practices.CQRS.Dispatching.Processors
{
	internal class RequestProcessor<TIn, TOut> : IProcessor
		where TIn : IRequest<TOut>
	{
		private readonly TypeResolver _resolver;

		public RequestProcessor(TypeResolver resolver)
		{
			_resolver = resolver;
		}

		public Task InvokeAsync(object args, CancellationToken cancellationToken)
		{
			return InvokeAsync((TIn)args, cancellationToken);
		}

		private async Task<TOut> InvokeAsync(TIn request, CancellationToken cancellationToken)
		{
			IRequestHandler<TIn, TOut> handler = ResolveHandlerOrThrow();

			foreach (var step in ResolvePreProcessors()) await step.PreProcessAsync(handler, request);

			TOut result = await handler.HandleAsync(request, cancellationToken);

			foreach (var step in ResolvePostProcessors()) await step.PostProcessAsync(handler, request, result);

			return result;
		}

		private IEnumerable<IRequestPostProcessor<TIn, TOut>> ResolvePostProcessors()
		{
			return _resolver.ResolveMany<IRequestPostProcessor<TIn, TOut>>();
		}

		private IEnumerable<IRequestPreProcessor<TIn, TOut>> ResolvePreProcessors()
		{
			return _resolver.ResolveMany<IRequestPreProcessor<TIn, TOut>>();
		}

		private static InvalidOperationException CreateHandlerNotFoundException()
		{
			return new InvalidOperationException($"Handler wasn't found for {typeof(TIn)} request.");
		}

		private IRequestHandler<TIn, TOut> ResolveHandlerOrThrow()
		{
			IRequestHandler<TIn, TOut> handler = _resolver.ResolveOne<IRequestHandler<TIn, TOut>>();
			if (handler != null)
				return handler;

			throw CreateHandlerNotFoundException();
		}
	}
}