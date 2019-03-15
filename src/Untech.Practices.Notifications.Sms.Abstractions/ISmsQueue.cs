using System.Threading.Tasks;

namespace Untech.Practices.Notifications.Sms
{
	/// <summary>
	///     Provides a set of methods for sending SMS into queue.
	/// </summary>
	public interface ISmsQueue
	{
		/// <summary>
		///     Adds the specified <paramref name="sms" /> into queue for sending.
		/// </summary>
		/// <param name="sms">Sms to send.</param>
		Task EnqueueAsync(Sms sms);
	}
}