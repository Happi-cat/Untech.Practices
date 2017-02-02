using System.Linq;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal class NotificationRequestExecutor<TIn> : IRequestExecutor
		where TIn : INotification
	{
		private readonly IHandlersResolver _resolver;

		public NotificationRequestExecutor(IHandlersResolver resolver)
		{
			_resolver = resolver;
		}

		public object Handle(object args)
		{
			var handlers = _resolver.ResolveHandlers<INotificationHandler<TIn>>();
			var input = (TIn) args;
			foreach (var handler in handlers)
			{
				handler.Publish(input);
			}

			return Unit.Value;
		}

		public Task HandleAsync(object args)
		{
			var handlers = _resolver.ResolveHandlers<INotificationAsyncHandler<TIn>>();
			var input = (TIn) args;
			var tasks = handlers
				.Select(n => n.PublishAsync(input))
				.ToArray();

			return Task.WhenAll(tasks);
		}
	}
}