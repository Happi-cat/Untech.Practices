using System.Collections.Generic;
using System.Threading.Tasks;

namespace Untech.Practices.Notifications
{
	/// <summary>
	///     Represents dummy notification queue.
	/// </summary>
	public class NullNotificationQueue<TNotification> : INotificationQueue<TNotification>
		where TNotification :INotification
	{
		/// <inheritdoc />
		public Task EnqueueAsync(TNotification notification)
		{
			return Task.CompletedTask;
		}

		public Task EnqueueAsync(IEnumerable<TNotification> notifications)
		{
			return Task.CompletedTask;
		}
	}
}