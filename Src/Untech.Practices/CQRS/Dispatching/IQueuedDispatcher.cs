using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Dispatching
{
	public interface IQueuedDispatcher
	{
		void Init(IDispatcher parent);

		void Enqueue<TResponse>(ICommand<TResponse> command, QueueOptions options);

		void Enqueue(INotification notification, QueueOptions options);
	}
}