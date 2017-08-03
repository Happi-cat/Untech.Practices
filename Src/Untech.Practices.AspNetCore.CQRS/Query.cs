using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.AspNetCore.CQRS.Mappers;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.Practices.AspNetCore.CQRS
{
	public static class Query
	{
		public static Query<TIn, TOut> Use<TIn, TOut>(IRequestMapper<TIn> requestBinder, IResponseMapper<TOut> responseBinder)
			where TIn : IQuery<TOut>
		{
			return new Query<TIn, TOut>(requestBinder, responseBinder);
		}

		public static QueryAsync<TIn, TOut> UseAsync<TIn, TOut>(IRequestMapper<TIn> requestBinder, IResponseMapper<TOut> responseBinder)
			where TIn : IQuery<TOut>
		{
			return new QueryAsync<TIn, TOut>(requestBinder, responseBinder);
		}
	}

	public class Query<TIn, TOut> : CqrsHandler<TIn, TOut>
		where TIn : IQuery<TOut>
	{
		public Query(IRequestMapper<TIn> requestBinder, IResponseMapper<TOut> responseBinder) : base(requestBinder, responseBinder)
		{
		}

		protected override Task<TOut> HandleCore(IDispatcher dispatcher, TIn data, CancellationToken cancellationToken)
		{
			return Task.FromResult(dispatcher.Fetch(data));
		}
	}
}
