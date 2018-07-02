namespace Untech.Practices.Notifications.Mail
{
	/// <summary>
	/// Represents dummy mail queue.
	/// </summary>
	public class NullMailQueue : IMailQueue
	{
		/// <inheritdoc />
		public void Enqueue(Mail mail)
		{
		}
	}
}
