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

		public class Notification : INotification
		{
		}

		public class Handler :
			IQueryHandler<Query, int>, IQueryAsyncHandler<Query, int>,
			ICommandHandler<Command, int>, ICommandAsyncHandler<Command, int>,
			INotificationHandler<Notification>, INotificationAsyncHandler<Notification>

		{
			public int Fetch(Query query) => 1;

			public Task<int> FetchAsync(Query query) => Task.FromResult(1);

			public int Process(Command command) => 1;

			public Task<int> ProcessAsync(Command command) => Task.FromResult(1);

			public void Publish(Notification notification)
			{
			}

			public Task PublishAsync(Notification notification)
			{
				return Task.CompletedTask;
			}
		}
	}
}
