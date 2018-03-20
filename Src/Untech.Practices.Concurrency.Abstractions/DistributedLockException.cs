using System;

namespace Untech.Practices.Concurrency.Abstractions
{
	public class DistributedLockException : Exception
	{
		public DistributedLockException(string message, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}