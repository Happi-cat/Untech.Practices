using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	///     Represents interface of a class that dispatches CQRS notifications to handlers.
	/// </summary>
	/// <remarks>
	///     <para>
	///         Supports <see cref="INotification" /> CQRS request.
	///     </para>
	/// </remarks>
	public interface INotificationDispatcher
	{
		/// <summary>
		///     Publishes the incoming <paramref name="notification" />.
		/// </summary>
		/// <param name="notification">The command to be published.</param>
		void Publish(INotification notification);

		/// <summary>
		///     Publishes asynchronously the incoming <paramref name="notification" />.
		/// </summary>
		/// <param name="notification">The command to be published.</param>
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		Task PublishAsync(INotification notification, CancellationToken cancellationToken);
	}
}