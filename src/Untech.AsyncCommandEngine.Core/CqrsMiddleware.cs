using System.Threading.Tasks;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncCommandEngine
{
	internal class CqrsMiddleware : IAceProcessorMiddleware
	{
		private readonly IDispatcher _dispatcher;

		public CqrsMiddleware(IDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		public async Task ExecuteAsync(AceContext context, AceRequestProcessorDelegate next)
		{
//			await _dispatcher.ProcessAsync(new DummyCommand(), context.RequestAborted);
		}
	}
}