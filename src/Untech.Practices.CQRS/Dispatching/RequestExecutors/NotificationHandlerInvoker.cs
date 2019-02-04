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

		public object Invoke(object args)
		{
			InvokeAsync(args, CancellationToken.None)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();

			return Nothing.AtAll;
		}

		public Task InvokeAsync(object args, CancellationToken cancellationToken)
		{
			TIn input = (TIn)args;

			IEnumerable<Task> syncHandlers = _resolver
				.ResolveMany<INotificationHandler<TIn>>()
				.Select(RunSync);

			IEnumerable<Task> asyncHandlers = _resolver
				.ResolveMany<INotificationAsyncHandler<TIn>>()
				.Select(RunAsync);

			return Task.WhenAll(syncHandlers.Concat(asyncHandlers));

			Task RunSync(INotificationHandler<TIn> handler)
			{
				return Task.Run(() => handler.Publish(input), cancellationToken);
			}

			Task RunAsync(INotificationAsyncHandler<TIn> handler)
			{
				return handler.PublishAsync(input, cancellationToken);
			}
		}
	}
}