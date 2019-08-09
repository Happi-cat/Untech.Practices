using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.CQRS.Pipeline;

namespace Untech.Practices.CQRS.Dispatching.Processors
{
	internal static class RequestProcessor<TIn, TOut> where TIn : IRequest<TOut>
	{
		public static Task InvokeAsync(TypeResolver resolver, object args, CancellationToken cancellationToken)
		{
			return InvokeAsync(resolver, (TIn)args, cancellationToken);
		}

		private static async Task<TOut> InvokeAsync(TypeResolver resolver, TIn request, CancellationToken cancellationToken)
		{
			var handler = resolver.ResolveOne<IRequestHandler<TIn, TOut>>()
				?? throw CreateHandlerNotFoundException();

			foreach (var step in resolver.ResolveMany<IRequestPreProcessor<TIn, TOut>>())
				await step.PreProcessAsync(handler, request);

			TOut result = await handler.HandleAsync(request, cancellationToken);

			foreach (var step in resolver.ResolveMany<IRequestPostProcessor<TIn, TOut>>())
				await step.PostProcessAsync(handler, request, result);

			return result;
		}

		private static InvalidOperationException CreateHandlerNotFoundException()
		{
			return new InvalidOperationException($"Handler wasn't found for {typeof(TIn)} request.");
		}
	}
}