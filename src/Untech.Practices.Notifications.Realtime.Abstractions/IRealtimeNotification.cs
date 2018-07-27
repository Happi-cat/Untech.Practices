namespace Untech.Practices.Notifications.Realtime
{
	/// <summary>
	/// Represents a common interface for realtime notifications of different types, like:
	/// PlatformNotification, TenantNotification, UserNotification, etc.
	/// </summary>
	public interface IRealtimeNotification
	{
		/// <summary>
		/// Gets payload that should be send.
		/// </summary>
		object Payload { get; }
	}
}