﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Dispatching.Processors
{
	internal class NotificationProcessor<TIn> : IProcessor
		where TIn : INotification
	{
		private readonly ITypeResolver _resolver;

		public NotificationProcessor(ITypeResolver resolver)
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