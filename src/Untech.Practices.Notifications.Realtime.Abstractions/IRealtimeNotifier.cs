using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Notifications.Realtime
{
	/// <summary>
	/// Provides methods for sending notifications asynchronously of a predefined type <typeparamref name="TNotification"/>.
	/// </summary>
	/// <typeparam name="TNotification"></typeparam>
	public interface IRealtimeNotifier<in TNotification>
		where TNotification : IRealtimeNotification
	{
		/// <summary>
		/// Sends the <paramref name="notification"/> to clients asynchronously.
		/// </summary>
		/// <param name="notification">The notification to send.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns></returns>
		Task SendAsync(TNotification notification, CancellationToken cancellationToken = default);
	}
}