﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Concurrency
{
	/// <summary>
	///     Defines methods for acquiring a distributed lock on a resource.
	/// </summary>
	public interface IDistributedLock
	{
		/// <summary>
		///     Tries to acquire distributed lock asynchronously for the specified <paramref name="resource" />.
		/// </summary>
		/// <param name="resource">Resource name to lock.</param>
		/// <param name="expiryTime">Expiration time for preventing of infinite locks.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Disposable lock if was acquired; otherwise null.</returns>
		Task<IDisposable> TryAcquireAsync(string resource, TimeSpan expiryTime,
			CancellationToken cancellationToken = default);
	}
}