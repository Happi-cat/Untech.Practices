using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS
{
	/// <summary>
	/// Represents interface of a class that dispatches CQRS requests to handlers and returns execution results.
	/// </summary>
	/// <remarks>
	/// <para>Supports <see cref="IQuery{TResult}"/>, <see cref="ICommand{TResponse}"/>, <see cref="INotification"/> CQRS requests.</para>
	/// </remarks>
	public interface IDispatcher
	{
		/// <summary>
		/// Gets instance of the <see cref="IQueueDispatcher"/> that enqueues commands and notification in execution queue.
		/// </summary>
		IQueueDispatcher Queue { get; }

		/// <summary>
		/// Fetches data of type <typeparamref name="TResult"/> for the specified <paramref name="query"/>.
		/// </summary>
		/// <typeparam name="TResult">The type of result.</typeparam>
		/// <param name="query">The query to be processed.</param>
		/// <returns>Matching data.</returns>
		TResult Fetch<TResult>(IQuery<TResult> query);

		/// <summary>
		/// Fetches asynchronously data of type <typeparamref name="TResult"/> for the specified <paramref name="query"/>.
		/// </summary>
		/// <typeparam name="TResult">The type of result.</typeparam>
		/// <param name="query">The query to be processed.</param>
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		/// <returns>Matching data.</returns>
		Task<TResult> FetchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);

		/// <summary>
		/// Processes the incoming <paramref name="command"/> and returns execution result.
		/// </summary>
		/// <typeparam name="TResult">The type of result.</typeparam>
		/// <param name="command">The command to be processed.</param>
		/// <returns>Execution result.</returns>
		TResult Process<TResult>(ICommand<TResult> command);

		/// <summary>
		/// Processes asynchronously the incoming <paramref name="command"/> and returns execution result.
		/// </summary>
		/// <typeparam name="TResult">The type of result.</typeparam>
		/// <param name="command">The command to be processed.</param>
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		/// <returns>Execution result.</returns>
		Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken);

		/// <summary>
		/// Publishes the incoming <paramref name="notification"/>.
		/// </summary>
		/// <param name="notification">The command to be published.</param>
		void Publish(INotification notification);

		/// <summary>
		/// Publishes asynchronously the incoming <paramref name="notification"/>.
		/// </summary>
		/// <param name="notification">The command to be published.</param>
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		Task PublishAsync(INotification notification, CancellationToken cancellationToken);
	}
}