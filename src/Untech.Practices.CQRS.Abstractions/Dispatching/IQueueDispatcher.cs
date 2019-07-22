using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	///     Represents interface of a dispatcher class that can put CQRS requests in execution queue.
	/// </summary>
	public interface IQueueDispatcher
	{
		/// <summary>
		///     Puts the incoming <paramref name="command" /> in execution queue and schedules execution using
		///     <paramref name="options" />.
		///     This method can be used when command can be executed separately or with a delay in another execution thread
		///     and there is no need to wait for result.
		/// </summary>
		/// <typeparam name="TResult">The type of result.</typeparam>
		/// <param name="command">The command to be enqueued.</param>
		/// <param name="options">The options that can be used for operation execution scheduling.</param>
		Task EnqueueAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default,
			QueueOptions options = null);

		/// <summary>
		///     Puts the incoming <paramref name="notification" /> in execution queue and schedules execution using
		///     <paramref name="options" />.
		///     This method can be used when command can be executed separately or with a delay in another execution thread
		///     and there is no need to wait for result.
		/// </summary>
		/// <param name="notification">The notification  to be enqueued.</param>
		/// <param name="options">The options that can be used for operation execution scheduling.</param>
		Task EnqueueAsync(INotification notification, CancellationToken cancellationToken = default,
			QueueOptions options = null);

		/// <summary>
		///     Puts the incoming <paramref name="notifications" /> in execution queue and schedules execution using
		///     <paramref name="options" />.
		///     This method can be used when command can be executed separately or with a delay in another execution thread
		///     and there is no need to wait for result.
		/// </summary>
		/// <param name="notifications">The notification  to be enqueued.</param>
		/// <param name="options">The options that can be used for operation execution scheduling.</param>
		Task EnqueueAsync(IEnumerable<INotification> notifications, CancellationToken cancellationToken = default,
			QueueOptions options = null);
	}
}