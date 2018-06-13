using System;
using System.Collections.Generic;
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
		private readonly IReadOnlyCollection<IPipelinePreProcessor<TIn>> _preProcessors;
		private readonly IReadOnlyCollection<IPipelinePostProcessor<TIn, TOut>> _postProcessors;

		public RequestHandlerInvoker(ITypeResolver resolver)
		{
			_resolver = resolver;

			_preProcessors = ResolveMany<IPipelinePreProcessor<TIn>>(resolver);
			_postProcessors = ResolveMany<IPipelinePostProcessor<TIn, TOut>>(resolver);
		}

		public  object Invoke(object args)
		{
			var syncHandler = _resolver.ResolveOne<IRequestHandler<TIn, TOut>>();
			if (syncHandler != null)
			{
				return Invoke(syncHandler, (TIn)args);
			}

			var asyncHandler = _resolver.ResolveOne<IRequestAsyncHandler<TIn, TOut>>();
			if (asyncHandler != null)
			{
				return InvokeAsync(asyncHandler, (TIn)args, CancellationToken.None)
					.ConfigureAwait(false)
					.GetAwaiter()
					.GetResult();
			}

			throw CreateHandlerNotFoundException();
		}

		public  Task InvokeAsync(object args, CancellationToken cancellationToken)
		{
			var asyncHandler = _resolver.ResolveOne<IRequestAsyncHandler<TIn, TOut>>();
			if (asyncHandler != null)
			{
				return InvokeAsync(asyncHandler, (TIn)args, cancellationToken);
			}

			var syncHandler = _resolver.ResolveOne<IRequestHandler<TIn, TOut>>();
			if (syncHandler != null)
			{
				return Task.FromResult(Invoke(syncHandler, (TIn)args));
			}

			throw CreateHandlerNotFoundException();
		}

		private static InvalidOperationException CreateHandlerNotFoundException()
		{
			return new InvalidOperationException($"Handler wasn't found for {typeof(TIn)} request.");
		}

		private TOut Invoke(IRequestHandler<TIn, TOut> handler, TIn request)
		{
			PreProcess(request);

			var result = handler.Handle(request);

			PostProcess(request, result);

			return result;
		}

		private async Task<TOut> InvokeAsync(IRequestAsyncHandler<TIn, TOut> handler, TIn request, CancellationToken cancellationToken)
		{
			PreProcess(request);

			var result = await handler.HandleAsync(request, cancellationToken);

			PostProcess(request, result);

			return result;
		}

		private void PreProcess(TIn request)
		{
			if (_preProcessors == null) return;

			foreach (var preProcessor in _preProcessors.Where(n => n != null))
			{
				preProcessor.Process(request);
			}
		}

		private void PostProcess(TIn request, TOut result)
		{
			if (_postProcessors == null) return;

			foreach (var postProcessor in _postProcessors.Where(n => n != null))
			{
				postProcessor.Process(request, result);
			}
		}

		private static IReadOnlyCollection<T> ResolveMany<T>(ITypeResolver resolver)
			where T : class
		{
			return (resolver.ResolveMany<T>() ?? Enumerable.Empty<T>())
				.Where(n => n != null)
				.ToList();
		}
	}
}