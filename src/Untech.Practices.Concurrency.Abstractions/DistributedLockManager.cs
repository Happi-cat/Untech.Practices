using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Concurrency
{
	/// <summary>
	///     Manager that implements <see cref="IDistributedLockManager" /> and
	///     provides methods for acquiring a ditributed lock on a resource using <see cref="IDistributedLock" /> as a
	///     lock-provider.
	/// </summary>
	public class DistributedLockManager : IDistributedLockManager
	{
		private readonly IDistributedLock _distributedLock;

		/// <summary>
		///     Initializes a new instance of the <see cref="DistributedLockManager" />.
		/// </summary>
		/// <param name="distributedLock">Distributed lock to use as a provider.</param>
		public DistributedLockManager(IDistributedLock distributedLock)
		{
			_distributedLock = distributedLock;
		}

		/// <inheritdoc />
		public IDisposable Acquire(string resource, LockOptions options)
		{
			IDisposable acquiredLock;
			if (options.WaitTime.HasValue)
			{
				acquiredLock = RepeatUntilAcquiredOrTimeouted(resource, options);

				if (acquiredLock == null) throw new DistributedLockTimeoutException(resource);
			}
			else
			{
				acquiredLock = _distributedLock.TryAcquire(resource, options.ExpiryTime);
			}

			return acquiredLock ?? throw new DistributedLockException(resource);
		}

		/// <inheritdoc />
		public async Task<IDisposable> AcquireAsync(string resource, LockOptions options,
			CancellationToken cancellationToken = default)
		{
			IDisposable acquiredLock;
			if (options.WaitTime.HasValue)
			{
				acquiredLock = await RepeatUntilAcquiredOrTimeoutedAsync(resource, options, cancellationToken);

				if (acquiredLock == null) throw new DistributedLockTimeoutException(resource);
			}
			else
			{
				acquiredLock = await _distributedLock.TryAcquireAsync(resource, options.ExpiryTime, cancellationToken);
			}

			return acquiredLock ?? throw new DistributedLockException(resource);
		}

		/// <inheritdoc />
		public IDisposable TryAcquire(string resource, LockOptions options)
		{
			return options.WaitTime.HasValue
				? RepeatUntilAcquiredOrTimeouted(resource, options)
				: _distributedLock.TryAcquire(resource, options.ExpiryTime);
		}

		/// <inheritdoc />
		public Task<IDisposable> TryAcquireAsync(string resource, LockOptions options,
			CancellationToken cancellationToken = default)
		{
			return options.WaitTime.HasValue
				? RepeatUntilAcquiredOrTimeoutedAsync(resource, options, cancellationToken)
				: _distributedLock.TryAcquireAsync(resource, options.ExpiryTime, cancellationToken);
		}

		private IDisposable RepeatUntilAcquiredOrTimeouted(string resource, LockOptions options)
		{
			TimeSpan wait = options.WaitTime.Value;
			TimeSpan retry = options.RetryTime ?? TimeSpan.FromMilliseconds(50);

			Stopwatch sw = new Stopwatch();
			sw.Start();

			while (sw.Elapsed < wait)
			{
				IDisposable acquiredLock = _distributedLock.TryAcquire(resource, options.ExpiryTime);
				if (acquiredLock != null) return acquiredLock;

				Thread.Sleep(retry);
			}

			return null;
		}

		private async Task<IDisposable> RepeatUntilAcquiredOrTimeoutedAsync(string resource, LockOptions options,
			CancellationToken cancellationToken)
		{
			TimeSpan wait = options.WaitTime.Value;
			TimeSpan retry = options.RetryTime ?? TimeSpan.FromMilliseconds(50);

			Stopwatch sw = new Stopwatch();
			sw.Start();

			while (sw.Elapsed < wait)
			{
				IDisposable acquiredLock =
					await _distributedLock.TryAcquireAsync(resource, options.ExpiryTime, cancellationToken);
				if (acquiredLock != null) return acquiredLock;

				await Task.Delay(retry, cancellationToken);
			}

			return null;
		}
	}
}