using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Dispatching;
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

		public class Notification : INotification
		{
		}

		public class Handler :
			IQueryHandler<Query, int>, IQueryAsyncHandler<Query, int>,
			ICommandHandler<Command, int>, ICommandAsyncHandler<Command, int>,
			INotificationHandler<Notification>, INotificationAsyncHandler<Notification>

		{
			public int Fetch(Query query) => 1;

			public Task<int> FetchAsync(Query query, CancellationToken cancellationToken) => Task.FromResult(1);

			public int Process(Command command) => 1;

			public Task<int> ProcessAsync(Command command, CancellationToken cancellationToken) => Task.FromResult(1);

			public void Publish(Notification notification)
			{
			}

			public Task PublishAsync(Notification notification, CancellationToken cancellationToken)
			{
				return Task.CompletedTask;
			}
		}

		public class Resolver : IHandlersResolver
		{
			public T ResolveHandler<T>() where T : class
			{
				var handler = new Handler();
				return handler as T;
			}

			public IEnumerable<T> ResolveHandlers<T>() where T : class
			{
				yield return ResolveHandler<T>();
			}
		}
	}
}
