using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.AspNetCore.CQRS.Mappers;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.Practices.AspNetCore.CQRS
{
	public class CommandAsync<TIn, TOut> : CqrsHandler<TIn, TOut>
		where TIn : ICommand<TOut>
	{
		public CommandAsync(IRequestMapper<TIn> requestBinder, IResponseMapper<TOut> responseBinder) : base(requestBinder, responseBinder)
		{
		}

		protected override Task<TOut> HandleCore(IDispatcher dispatcher, TIn data, CancellationToken cancellationToken)
		{
			return dispatcher.ProcessAsync(data, cancellationToken);
		}
	}
}
