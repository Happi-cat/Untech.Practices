using System;

namespace Untech.Practices.Concurrency
{
	/// <summary>
	///     Represents errors that occur when distributed lock wasn't acquired in a specified amount of time.
	/// </summary>
	public class DistributedLockTimeoutException : TimeoutException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="DistributedLockTimeoutException" />.
		/// </summary>
		/// <param name="resource">Resource name to lock.</param>
		/// <param name="innerException">Inner exception.</param>
		public DistributedLockTimeoutException(string resource, Exception innerException = null)
			: base($"Distributed lock for resource {resource} was not acquired in time")
		{
		}
	}
}