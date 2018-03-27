﻿using System;

namespace Untech.Practices.Concurrency.Abstractions
{
	/// <summary>
	/// Represents errors that occurs when distributed lock wasn't acquired.
	/// </summary>
	public class DistributedLockException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DistributedLockException"/>.
		/// </summary>
		/// <param name="resource">Resource name to lock.</param>
		/// <param name="innerException">Inner exception.</param>
		public DistributedLockException(string resource, Exception innerException = null)
			: base($"Distributed lock for resource {resource} was not acquired", innerException)
		{
		}
	}
}