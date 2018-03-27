using System;

namespace Untech.Practices.Concurrency
{
	/// <summary>
	/// Options for <see cref="IDistributedLockManager"/> for lock acquiring.
	/// </summary>
	public class LockOptions
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LockOptions"/> with specified <paramref name="expiryTime"/>.
		/// </summary>
		/// <param name="expiryTime">Expiration time for preventing invinite locks.</param>
		public LockOptions(TimeSpan expiryTime)
		{
			ExpiryTime = expiryTime;
		}

		/// <summary>
		/// Gets expiration time.
		/// </summary>
		public TimeSpan ExpiryTime { get; }
		/// <summary>
		/// Gets or sets timeout for lock acquiring.
		/// </summary>
		public TimeSpan? WaitTime { get; set; }
		/// <summary>
		/// Gets or sets delay between retries.
		/// </summary>
		public TimeSpan? RetryTime { get; set; }
	}
}