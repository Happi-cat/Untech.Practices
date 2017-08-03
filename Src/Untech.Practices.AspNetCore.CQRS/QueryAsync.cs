using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.AspNetCore.CQRS.Mappers;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.Practices.AspNetCore.CQRS
{
	public class QueryAsync<TIn, TOut> : CqrsHandler<TIn, TOut>
		where TIn : IQuery<TOut>
	{
		public QueryAsync(IRequestMapper<TIn> requestBinder, IResponseMapper<TOut> responseBinder) : base(requestBinder, responseBinder)
		{
		}

		protected override Task<TOut> HandleCore(IDispatcher dispatcher, TIn data, CancellationToken cancellationToken)
		{
			return dispatcher.FetchAsync(data, cancellationToken);
		}
	}
}
