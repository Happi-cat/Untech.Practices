using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Untech.Practices.Notifications
{
	public class SimpleNotificationQueue<T> : INotificationQueue<T> where T : INotification
	{
		private readonly IRealtimeNotifier<T> _realtimeNotifier;

		public SimpleNotificationQueue(IRealtimeNotifier<T> realtimeNotifier)
		{
			_realtimeNotifier = realtimeNotifier ?? throw new ArgumentNullException(nameof(realtimeNotifier));
		}

		public Task EnqueueAsync(T notification)
		{
			return Task.Run(() => _realtimeNotifier.SendAsync(notification));
		}

		public Task EnqueueAsync(IEnumerable<T> notifications)
		{
			return Task.Run(() => _realtimeNotifier.SendAsync(notifications));
		}
	}
}