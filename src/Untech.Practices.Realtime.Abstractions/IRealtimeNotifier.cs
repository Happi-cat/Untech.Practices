using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Realtime
{
	public interface IRealtimeNotifier<in TNotification>
		where TNotification : IRealtimeNotification
	{
		void Notify(TNotification notification);
		Task NotifyAsync(TNotification notification, CancellationToken cancellationToken);
	}
}