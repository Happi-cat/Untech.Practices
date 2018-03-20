using System;

namespace Untech.Practices.Concurrency.Abstractions
{
	public class AcquireOptions
	{
		public AcquireOptions(TimeSpan expiryTime)
		{
			ExpiryTime = expiryTime;
		}

		public TimeSpan ExpiryTime { get; }
		public TimeSpan? WaitTime { get; set; }
		public TimeSpan? RetryTime { get; set; }
	}
}