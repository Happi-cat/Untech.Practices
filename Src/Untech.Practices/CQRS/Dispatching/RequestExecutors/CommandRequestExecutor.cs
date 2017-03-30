using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal class CommandRequestExecutor<TIn, TOut> : IRequestExecutor
		where TIn : ICommand<TOut>
	{
		private readonly IHandlersResolver _resolver;

		public CommandRequestExecutor(IHandlersResolver resolver)
		{
			_resolver = resolver;
		}

		public object Handle(object args)
		{
			var handler = _resolver.ResolveHandler<ICommandHandler<TIn, TOut>>();
			return handler.Process((TIn) args);
		}

		public Task HandleAsync(object args)
		{
			var handler = _resolver.ResolveHandler<ICommandAsyncHandler<TIn, TOut>>();
			return handler.ProcessAsync((TIn) args);
		}
	}
}