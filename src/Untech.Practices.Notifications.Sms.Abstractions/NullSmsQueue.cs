namespace Untech.Practices.Notifications.Sms
{
	/// <summary>
	/// Represents dummy sms queue.
	/// </summary>
	public class NullSmsQueue : ISmsQueue
	{
		/// <inheritdoc />
		public void Enqueue(Sms sms)
		{
		}
	}
}