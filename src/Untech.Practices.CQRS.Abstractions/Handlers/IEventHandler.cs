using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	///     Defines an async handler for a notification.
	/// </summary>
	/// <typeparam name="TIn">Notification type</typeparam>
	public interface IEventHandler<in TIn>
		where TIn : IEvent
	{
		/// <summary>
		///     Publishes notification asynchronously.
		/// </summary>
		/// <param name="notification">Notification to be handled.</param>
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		Task PublishAsync(TIn notification, CancellationToken cancellationToken);
	}
}