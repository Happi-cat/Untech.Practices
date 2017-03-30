namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Defines a handler for a notification.
	/// </summary>
	/// <typeparam name="TIn">Notification type</typeparam>
	public interface INotificationHandler<in TIn>
		where TIn : INotification
	{
		/// <summary>
		/// Publishes notification.
		/// </summary>
		/// <param name="notification">Notification to be handled.</param>
		void Publish(TIn notification);
	}
}