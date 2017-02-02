using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal class QueryRequestExecutor<TIn, TOut> : IRequestExecutor
		where TIn: IQuery<TOut>
	{
		private readonly IHandlersResolver _resolver;

		public QueryRequestExecutor(IHandlersResolver resolver)
		{
			_resolver = resolver;
		}

		public object Handle(object args)
		{
			var handler = _resolver.ResolveHandler<IQueryHandler<TIn, TOut>>();
			return handler.Fetch((TIn) args);
		}

		public Task HandleAsync(object args)
		{
			var handler = _resolver.ResolveHandler<IQueryAsyncHandler<TIn, TOut>>();
			return handler.FetchAsync((TIn)args);
		}
	}
}