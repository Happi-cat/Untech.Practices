using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncCommandEngine.Run;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Handlers;

namespace AsyncCommandEngine.Examples
{
	public class DummyCommand : ICommand<int>
	{
	}

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