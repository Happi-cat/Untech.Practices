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
		/// <returns>Matching data.</returns>
		Task<TResult> FetchAsync<TResult>(IQuery<TResult> query);

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
		/// <returns>Execution result.</returns>
		Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command);

		/// <summary>
		/// Publishes the incoming <paramref name="notification"/>.
		/// </summary>
		/// <param name="notification">The command to be published.</param>
		void Publish(INotification notification);

		/// <summary>
		/// Publishes asynchronously the incoming <paramref name="notification"/>.
		/// </summary>
		/// <param name="notification">The command to be published.</param>
		Task PublishAsync(INotification notification);

		/// <summary>
		/// Puts the incoming <paramref name="command"/> in execution queue and schedules execution using <paramref name="options"/>.
		/// This method can be used when command can be executed separately or with a delay in another execution thread 
		/// and there is no need to wait for result.
		/// </summary>
		/// <typeparam name="TResult">The type of result.</typeparam>
		/// <param name="command">The command to be enqueued.</param>
		/// <param name="options">The options that can be used for operation execution scheduling.</param>
		void Enqueue<TResult>(ICommand<TResult> command, QueueOptions options = null);

		/// <summary>
		/// Puts the incoming <paramref name="notification"/> in execution queue and schedules execution using <paramref name="options"/>.
		/// This method can be used when command can be executed separately or with a delay in another execution thread 
		/// and there is no need to wait for result.
		/// </summary>
		/// <param name="notification">The notification  to be enqueued.</param>
		/// <param name="options">The options that can be used for operation execution scheduling.</param>
		void Enqueue(INotification notification, QueueOptions options = null);
	}
}