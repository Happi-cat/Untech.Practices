using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Concurrency.Abstractions
{
	public class DistributedLockManager : IDistributedLockManager
	{
		private readonly IDistributedLock _distributedLock;

		public DistributedLockManager(IDistributedLock distributedLock)
		{
			_distributedLock = distributedLock;
		}

		public IDisposable Acquire(string resource, AcquireOptions options)
		{
			IDisposable acquiredLock;
			if (options.WaitTime.HasValue)
			{
				acquiredLock = RepeatUntilAcquiredOrTimeouted(resource, options);

				if (acquiredLock == null)
				{
					throw new TimeoutException($"Lock not acquired for resource {resource} in {options.WaitTime}");
				}
			}
			else
			{
				acquiredLock = _distributedLock.TryAcquire(resource, options.ExpiryTime);
			}

			return acquiredLock ?? throw new DistributedLockException($"Lock not acquired for resource {resource}");
		}

		public async Task<IDisposable> AcquireAsync(string resource, AcquireOptions options,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			IDisposable acquiredLock;
			if (options.WaitTime.HasValue)
			{
				acquiredLock = await RepeatUntilAcquiredOrTimeoutedAsync(resource, options, cancellationToken);

				if (acquiredLock == null)
				{
					throw new TimeoutException($"Lock not acquired for resource {resource} in {options.WaitTime}");
				}
			}
			else
			{
				acquiredLock = await _distributedLock.TryAcquireAsync(resource, options.ExpiryTime, cancellationToken);
			}

			return acquiredLock ?? throw new DistributedLockException($"Lock not acquired for resource {resource}");
		}

		public IDisposable TryAcquire(string resource, AcquireOptions options)
		{
			return options.WaitTime.HasValue
				? RepeatUntilAcquiredOrTimeouted(resource, options)
				: _distributedLock.TryAcquire(resource, options.ExpiryTime);
		}

		public Task<IDisposable> TryAcquireAsync(string resource, AcquireOptions options,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			return options.WaitTime.HasValue
				? RepeatUntilAcquiredOrTimeoutedAsync(resource, options, cancellationToken)
				: _distributedLock.TryAcquireAsync(resource, options.ExpiryTime, cancellationToken);
		}

		private IDisposable RepeatUntilAcquiredOrTimeouted(string resource, AcquireOptions options)
		{
			var waitMills = options.WaitTime.Value.Milliseconds;
			var retryMills = options.RetryTime?.Milliseconds ?? 50;

			var sw = new Stopwatch();
			sw.Start();

			while (sw.ElapsedMilliseconds < waitMills)
			{
				var acquiredLock = _distributedLock.TryAcquire(resource, options.ExpiryTime);
				if (acquiredLock != null)
				{
					return acquiredLock;
				}

				Thread.Sleep(retryMills);
			}

			return null;
		}

		private async Task<IDisposable> RepeatUntilAcquiredOrTimeoutedAsync(string resource, AcquireOptions options,
			CancellationToken cancellationToken)
		{
			var waitMills = options.WaitTime.Value.Milliseconds;
			var retryMills = options.RetryTime?.Milliseconds ?? 50;

			var sw = new Stopwatch();
			sw.Start();

			while (sw.ElapsedMilliseconds < waitMills)
			{
				var acquiredLock = await _distributedLock.TryAcquireAsync(resource, options.ExpiryTime, cancellationToken);
				if (acquiredLock != null)
				{
					return acquiredLock;
				}

				await Task.Delay(retryMills, cancellationToken);
			}

			return null;
		}
	}
}