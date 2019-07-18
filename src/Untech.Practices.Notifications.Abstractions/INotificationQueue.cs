using System.Threading.Tasks;

namespace Untech.Practices.Notifications
{
	/// <summary>
	/// Defines methods for sending notifications into queue.
	/// </summary>
	public interface INotificationQueue<in TNotification>
		where TNotification: INotification
	{
		/// <summary>
		/// Adds notification for sending into queue.
		/// </summary>
		/// <param name="notification">The notification to sent.</param>
		Task EnqueueAsync(TNotification notification);
	}
}
