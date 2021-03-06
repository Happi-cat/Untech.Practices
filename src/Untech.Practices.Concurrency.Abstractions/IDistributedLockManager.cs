﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Concurrency
{
	/// <summary>
	///     Defines methods for acquiring distributed lock with advanced control.
	/// </summary>
	public interface IDistributedLockManager
	{
		/// <summary>
		///     Acquires distributed lock asynchronously for the specified <paramref name="resource" /> or throws exception.
		/// </summary>
		/// <param name="resource">Resource name to lock.</param>
		/// <param name="options">Lock options to use.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Disposable lock.</returns>
		/// <exception cref="DistributedLockNotAcquiredException">Lock was not acquired.</exception>
		Task<IDisposable> AcquireAsync(string resource, LockOptions options,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Tries to acquire distributed lock asynchronously for the specified <paramref name="resource" /> or throws
		///     exception.
		/// </summary>
		/// <param name="resource">Resource name to lock.</param>
		/// <param name="options">Lock options to use.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Distributed lock if was acquired; otherwise null.</returns>
		Task<IDisposable> TryAcquireAsync(string resource, LockOptions options,
			CancellationToken cancellationToken = default);
	}
}