using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal class NotificationHandlerInvoker<TIn> : IHandlerInvoker
		where TIn : INotification
	{
		private readonly ITypeResolver _resolver;

		public NotificationHandlerInvoker(ITypeResolver resolver)
		{
			_resolver = resolver;
		}

		public Task InvokeAsync(object args, CancellationToken cancellationToken)
		{
			TIn input = (TIn)args;

			IEnumerable<Task> asyncHandlers = _resolver
				.ResolveMany<INotificationHandler<TIn>>()
				.Select(RunAsync);

			return Task.WhenAll(asyncHandlers);

			Task RunAsync(INotificationHandler<TIn> handler)
			{
				return handler.PublishAsync(input, cancellationToken);
			}
		}
	}
}