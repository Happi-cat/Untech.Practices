using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Represents dummy notification handler.
	/// </summary>
	/// <typeparam name="TIn">The type of notification to be handled.</typeparam>
	public sealed class StubNotificationHandler<TIn> :
		INotificationHandler<TIn>, INotificationAsyncHandler<TIn>
		where TIn : INotification
	{
		/// <summary>
		/// Publishes notification.
		/// </summary>
		/// <param name="notification">Notification to be handled.</param>
		public void Publish(TIn notification)
		{
		}

		/// <summary>
		/// Publishes notification asynchronously.
		/// </summary>
		/// <param name="notification">Notification to be handled.</param>
		public Task PublishAsync(TIn notification) => Task.CompletedTask;
	}
}