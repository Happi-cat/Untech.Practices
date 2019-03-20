using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Features.WatchDog;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Handlers;

namespace Untech.AsyncCommandEngine
{
	public class DummyCommand : ICommand<int>
	{
	}

	[WatchDogTimeout(0, 10, 0)]
	public class DummyCommandHandler: ICommandHandler<DummyCommand, int>
	{
		public Task<int> HandleAsync(DummyCommand request, CancellationToken cancellationToken)
		{
			Console.WriteLine("Hello World!");
			return Task.FromResult(100);
		}
	}


	public class DummyEvent : INotification
	{

	}

	public class DummyEventHandler1 : INotificationHandler<DummyEvent>
	{
		public Task PublishAsync(DummyEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine("Hello World!");
			return Task.CompletedTask;
		}
	}

	public class DummyEventHandler2 : INotificationHandler<DummyEvent>
	{
		public Task PublishAsync(DummyEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine("Hello World!");
			return Task.CompletedTask;
		}
	}
}