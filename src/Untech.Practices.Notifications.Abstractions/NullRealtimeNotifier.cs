using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Notifications
{
	public class NullRealtimeNotifier<T> : IRealtimeNotifier<T> where T : INotification
	{
		public  Task SendAsync(T notification, CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}

		public  Task SendAsync(IEnumerable<T> notifications, CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}
	}
}