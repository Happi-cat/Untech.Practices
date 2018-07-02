using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Realtime
{
	/// <summary>
	/// Provides methods for sending notifications of a predefined type <typeparamref name="TNotification"/>.
	/// </summary>
	/// <typeparam name="TNotification"></typeparam>
	public interface IRealtimeNotifier<in TNotification>
		where TNotification : IRealtimeNotification
	{
		/// <summary>
		/// Sends the <paramref name="notification"/> to clients.
		/// </summary>
		/// <param name="notification">The notification to send.</param>
		void Send(TNotification notification);

		/// <summary>
		/// Sends the <paramref name="notification"/> to clients.
		/// </summary>
		/// <param name="notification">The notification to send.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns></returns>
		Task SendAsync(TNotification notification, CancellationToken cancellationToken = default);
	}
}