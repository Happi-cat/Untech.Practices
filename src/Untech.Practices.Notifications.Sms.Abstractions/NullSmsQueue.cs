using System.Threading.Tasks;

namespace Untech.Practices.Notifications.Sms
{
	/// <summary>
	///     Represents dummy sms queue.
	/// </summary>
	public class NullSmsQueue : ISmsQueue
	{
		/// <inheritdoc />
		public Task EnqueueAsync(Sms sms)
		{
			return Task.FromResult(0);
		}
	}
}