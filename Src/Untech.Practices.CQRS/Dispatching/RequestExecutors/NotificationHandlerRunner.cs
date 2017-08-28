using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal class NotificationHandlerRunner<TIn> : IHandlerRunner
		where TIn : INotification
	{
		private readonly ITypeResolver _resolver;
		private readonly ITypeInitializer _typeInitializer;

		public NotificationHandlerRunner(ITypeResolver resolver, ITypeInitializer typeInitializer)
		{
			_resolver = resolver;
			_typeInitializer = typeInitializer;
		}

		public object Handle(object args)
		{
			HandleAsync(args, CancellationToken.None).Wait();

			return Nothing.AtAll;
		}

		public Task HandleAsync(object args, CancellationToken cancellationToken)
		{
			var input = (TIn)args;

			var syncHandlers = _resolver.ResolveMany<INotificationHandler<TIn>>()
				.Select(handler => Handle(handler, input, cancellationToken));

			var asyncHandlers = _resolver.ResolveMany<INotificationAsyncHandler<TIn>>()
				.Select(handler => Handle(handler, input, cancellationToken));

			return Task.WhenAll(syncHandlers.Concat(asyncHandlers));
		}

		private Task Handle(INotificationHandler<TIn> handler, TIn input, CancellationToken cancellationToken)
		{
			_typeInitializer?.Init(handler, input);

			return Task.Run(() => handler.Publish(input), cancellationToken);
		}

		private Task Handle(INotificationAsyncHandler<TIn> handler, TIn input, CancellationToken cancellationToken)
		{
			_typeInitializer?.Init(handler, input);

			return handler.PublishAsync(input, cancellationToken);
		}
	}
}