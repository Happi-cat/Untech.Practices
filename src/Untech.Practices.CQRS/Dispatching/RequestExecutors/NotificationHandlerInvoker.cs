﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal class NotificationHandlerInvoker<TIn> : IHandlerInvoker
		where TIn : INotification
	{
		private readonly ITypeResolver _resolver;
		private readonly IHandlerInitializer _handlerInitializer;

		public NotificationHandlerInvoker(ITypeResolver resolver, IHandlerInitializer handlerInitializer)
		{
			_resolver = resolver;
			_handlerInitializer = handlerInitializer;
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
			var input = (TIn)args;

			var syncHandlers = _resolver.ResolveMany<INotificationHandler<TIn>>()
				.Select(handler => Handle(handler, input, cancellationToken));

			var asyncHandlers = _resolver.ResolveMany<INotificationAsyncHandler<TIn>>()
				.Select(handler => Handle(handler, input, cancellationToken));

			return Task.WhenAll(syncHandlers.Concat(asyncHandlers));
		}

		private Task Handle(INotificationHandler<TIn> handler, TIn input, CancellationToken cancellationToken)
		{
			_handlerInitializer?.Init(handler, input);

			return Task.Run(() => handler.Publish(input), cancellationToken);
		}

		private Task Handle(INotificationAsyncHandler<TIn> handler, TIn input, CancellationToken cancellationToken)
		{
			_handlerInitializer?.Init(handler, input);

			return handler.PublishAsync(input, cancellationToken);
		}
	}
}