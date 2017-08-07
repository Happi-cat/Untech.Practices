using System.Collections.Generic;
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
		private readonly IEnumerable<IPipelinePreProcessor<TIn>> _preProcessors;
		private readonly IEnumerable<IPipelinePostProcessor<TIn, TOut>> _postProcessors;

		protected RequestHandlerRunner(ITypeResolver resolver, ITypeInitializer typeInitializer)
		{
			_typeInitializer = typeInitializer;

			_preProcessors = resolver.ResolveMany<IPipelinePreProcessor<TIn>>();
			_postProcessors = resolver.ResolveMany<IPipelinePostProcessor<TIn, TOut>>();
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
			foreach (var preProcessor in _preProcessors)
			{
				preProcessor.Process(request);
			}
		}

		private void PostProcess(TIn request, TOut result)
		{
			foreach (var postProcessor in _postProcessors)
			{
				postProcessor.Process(request, result);
			}
		}
	}
}