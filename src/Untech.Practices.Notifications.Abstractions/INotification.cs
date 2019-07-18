namespace Untech.Practices.Notifications
{
	/// <summary>
	/// Represents a common interface for realtime notifications of different types, like:
	/// Mail, Sms, Realtime, etc.
	/// </summary>
	public interface INotification
	{
		/// <summary>
		/// Gets payload that should be send.
		/// </summary>
		object Payload { get; }
	}
}
