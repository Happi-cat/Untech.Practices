using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Defines an async handler for a notification.
	/// </summary>
	/// <typeparam name="TIn">Notification type</typeparam>
	public interface INotificationAsyncHandler<in TIn>
		where TIn : INotification
	{
		/// <summary>
		/// Publishes notification asynchronously.
		/// </summary>
		/// <param name="notification">Notification to be handled.</param>
		Task PublishAsync(TIn notification);
	}
}