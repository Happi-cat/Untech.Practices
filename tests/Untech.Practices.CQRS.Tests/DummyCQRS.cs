﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS
{
	public static class DummyCQRS
	{
		public class Query : IQuery<int>
		{
		}

		public class Command : ICommand<int>
		{
		}

		public class CommandWithUnknownHandler : ICommand<int>
		{
		}

		public class CommandWithError : ICommand<int>
		{

		}

		public class Event : IEvent
		{
		}

		public class Handler :
			IQueryHandler<Query, int>,
			ICommandHandler<Command, int>,
			ICommandHandler<CommandWithError, int>,
			IEventHandler<Event>
		{
			public Task<int> HandleAsync(Query query, CancellationToken cancellationToken) => Task.FromResult(1);

			public Task<int> HandleAsync(Command command, CancellationToken cancellationToken) => Task.FromResult(1);

			public Task PublishAsync(Event @event, CancellationToken cancellationToken)
			{
				return Task.CompletedTask;
			}

			public Task<int> HandleAsync(CommandWithError request, CancellationToken cancellationToken)
			{
				throw new System.NotImplementedException();
			}
		}

		public class Resolver : IServiceProvider
		{
			public object GetService(Type serviceType)
			{
				if (serviceType.IsAssignableFrom(typeof(Handler)))
					return new Handler();
				if (serviceType.IsAssignableFrom(typeof(IEnumerable<Handler>)))
					return new[] { new Handler() };
				if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
					return null;
				throw new InvalidOperationException();
			}
		}
	}
}
