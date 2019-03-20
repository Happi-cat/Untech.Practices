using System.Threading.Tasks;
using AsyncCommandEngine.Examples;
using Untech.AsyncCommmandEngine.Abstractions;
using Untech.Practices.CQRS.Dispatching;

namespace AsyncCommandEngine.Run
{
	internal class CqrsMiddleware : IAceProcessorMiddleware
	{
		private readonly IDispatcher _dispatcher;

		public CqrsMiddleware(IDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		public async Task Execute(AceContext context, AceRequestProcessorDelegate next)
		{
			await new DummyCommandHandler().HandleAsync(new DummyCommand(), context.RequestAborted);
//			await _dispatcher.ProcessAsync(new DummyCommand(), context.RequestAborted);
		}
	}
}