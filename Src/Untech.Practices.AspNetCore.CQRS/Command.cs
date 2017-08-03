using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.AspNetCore.CQRS.Mappers;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.Practices.AspNetCore.CQRS
{
	public static class Command
	{
		public static Command<TIn, TOut> Use<TIn, TOut>(IRequestMapper<TIn> requestBinder, IResponseMapper<TOut> responseBinder)
			where TIn : ICommand<TOut>
		{
			return new Command<TIn, TOut>(requestBinder, responseBinder);
		}

		public static CommandAsync<TIn, TOut> UseAsync<TIn, TOut>(IRequestMapper<TIn> requestBinder, IResponseMapper<TOut> responseBinder)
			where TIn : ICommand<TOut>
		{
			return new CommandAsync<TIn, TOut>(requestBinder, responseBinder);
		}
	}

	public class Command<TIn, TOut> : CqrsHandler<TIn, TOut>
		where TIn : ICommand<TOut>
	{
		public Command(IRequestMapper<TIn> requestBinder, IResponseMapper<TOut> responseBinder) : base(requestBinder, responseBinder)
		{
		}

		protected override Task<TOut> HandleCore(IDispatcher dispatcher, TIn data, CancellationToken cancellationToken)
		{
			return Task.FromResult(dispatcher.Process(data));
		}
	}
}
