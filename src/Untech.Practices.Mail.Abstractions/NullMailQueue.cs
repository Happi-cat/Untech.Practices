namespace Untech.Practices.Mail
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