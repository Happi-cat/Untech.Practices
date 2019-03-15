using System.Threading.Tasks;

namespace Untech.Practices.Notifications.Mail
{
	/// <summary>
	///     Represents dummy mail queue.
	/// </summary>
	public class NullMailQueue : IMailQueue
	{
		/// <inheritdoc />
		public Task EnqueueAsync(Mail mail)
		{
			return Task.FromResult(0);
		}
	}
}