using System.Threading.Tasks;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Dispatching
{
	public interface IDispatcher
	{
		TResponse Fetch<TResponse>(IQuery<TResponse> query);

		Task<TResponse> FetchAsync<TResponse>(IQuery<TResponse> query);

		TResponse Process<TResponse>(ICommand<TResponse> command);

		Task<TResponse> ProcessAsync<TResponse>(ICommand<TResponse> command);

		void Publish(INotification notification);

		Task PublishAsync(INotification notification);

		void Enqueue<TResponse>(ICommand<TResponse> command, QueueOptions options = null);

		void Enqueue(INotification notification, QueueOptions options = null);
	}
}