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
			IQueryHandler<Query, int>, IQueryHandler<Query, int>,
			ICommandHandler<Command, int>, ICommandHandler<Command, int>,
			INotificationHandler<Notification>, INotificationHandler<Notification>

		{
			public int Handle(Query query) => 1;

			public Task<int> HandleAsync(Query query, CancellationToken cancellationToken) => Task.FromResult(1);

			public int Handle(Command command) => 1;

			public Task<int> HandleAsync(Command command, CancellationToken cancellationToken) => Task.FromResult(1);

			public void Publish(Notification notification)
			{
			}

			public Task PublishAsync(Notification notification, CancellationToken cancellationToken)
			{
				return Task.CompletedTask;
			}
		}

		public class Resolver : ITypeResolver
		{
			public T ResolveOne<T>() where T : class
			{
				var handler = new Handler();
				return handler as T;
			}

			public IEnumerable<T> ResolveMany<T>() where T : class
			{
				yield return ResolveOne<T>();
			}
		}
	}
}
