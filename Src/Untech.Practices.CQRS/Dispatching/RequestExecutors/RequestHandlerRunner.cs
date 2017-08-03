using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.CQRS.Pipeline;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal abstract class RequestHandlerRunner<TIn, TOut> : IHandlerRunner
		where TIn : IRequest<TOut>
	{
		private readonly ITypeResolver _resolver;
		private readonly ITypeInitializer _typeInitializer;

		protected RequestHandlerRunner(ITypeResolver resolver, ITypeInitializer typeInitializer)
		{
			_resolver = resolver;
			_typeInitializer = typeInitializer;
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
			foreach (var preProcessor in _resolver.ResolveMany<IPipelinePreProcessor<TIn>>())
			{
				preProcessor.Process(request);
			}
		}

		private void PostProcess(TIn request, TOut result)
		{
			foreach (var postProcessor in _resolver.ResolveMany<IPipelinePostProcessor<TIn, TOut>>())
			{
				postProcessor.Process(request, result);
			}
		}
	}
}