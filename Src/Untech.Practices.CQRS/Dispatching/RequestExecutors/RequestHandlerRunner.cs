using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.CQRS.Pipeline;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal abstract class RequestHandlerRunner<TIn, TOut> : IHandlerRunner
		where TIn : IRequest<TOut>
	{
		private readonly ITypeInitializer _typeInitializer;
		private readonly IReadOnlyCollection<IPipelinePreProcessor<TIn>> _preProcessors;
		private readonly IReadOnlyCollection<IPipelinePostProcessor<TIn, TOut>> _postProcessors;

		protected RequestHandlerRunner(ITypeResolver resolver, ITypeInitializer typeInitializer)
		{
			_typeInitializer = typeInitializer;

			_preProcessors = ResolveMany<IPipelinePreProcessor<TIn>>(resolver);
			_postProcessors = ResolveMany<IPipelinePostProcessor<TIn, TOut>>(resolver);
		}

		public abstract object Handle(object args);
		public abstract Task HandleAsync(object args, CancellationToken cancellationToken);

		protected TOut Handle(IRequestHandler<TIn, TOut> handler, TIn request)
		{
			PreProcess(request);

			_typeInitializer?.Init(handler, request);

			var result = handler.Handle(request);

			PostProcess(request, result);

			return result;
		}

		protected async Task<TOut> HandleAsync(IRequestAsyncHandler<TIn, TOut> handler, TIn request, CancellationToken cancellationToken)
		{
			PreProcess(request);

			_typeInitializer?.Init(handler, request);

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
			where T: class
		{
			return (resolver.ResolveMany<T>() ?? Enumerable.Empty<T>())
				.Where(n => n != null)
				.ToList();
		}
	}
}