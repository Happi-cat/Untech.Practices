using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Concurrency
{
	/// <summary>
	///     Manager that implements <see cref="IDistributedLockManager" /> and
	///     provides methods for acquiring a distributed lock on a resource using <see cref="IDistributedLock" /> as a
	///     lock-provider.
	/// </summary>
	public class DistributedLockManager : IDistributedLockManager
	{
		private readonly IDistributedLock _innerLock;

		/// <summary>
		///     Initializes a new instance of the <see cref="DistributedLockManager" />.
		/// </summary>
		/// <param name="innerLock">Distributed lock to use as a provider.</param>
		public DistributedLockManager(IDistributedLock innerLock)
		{
			_innerLock = innerLock;
		}

		/// <inheritdoc />
		public async Task<IDisposable> AcquireAsync(string resource, LockOptions options,
			CancellationToken cancellationToken = default)
		{
			IDisposable acquiredLock = await TryAcquireAsync(resource, options, cancellationToken);

			return acquiredLock ?? throw new DistributedLockNotAcquiredException(resource);
		}

		/// <inheritdoc />
		public Task<IDisposable> TryAcquireAsync(string resource, LockOptions options,
			CancellationToken cancellationToken = default)
		{
			return options.WaitTime.HasValue
				? TryRepeatAsync(resource, options, cancellationToken)
				: _innerLock.TryAcquireAsync(resource, options.ExpiryTime, cancellationToken);
		}

		private async Task<IDisposable> TryRepeatAsync(string resource, LockOptions options,
			CancellationToken cancellationToken)
		{
			TimeSpan wait = options.WaitTime ?? TimeSpan.Zero;
			TimeSpan retry = options.RetryTime ?? TimeSpan.FromMilliseconds(50);
			TimeSpan expiry = options.ExpiryTime;

			cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(
				new CancellationTokenSource(wait).Token,
				cancellationToken
			).Token;

			try { return await RepeatAsync(resource, retry, expiry, cancellationToken); }
			catch (TaskCanceledException) { return null; }
		}

		private async Task<IDisposable> RepeatAsync(string resource, TimeSpan retry, TimeSpan expiry,
			CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				IDisposable acquiredLock = await _innerLock.TryAcquireAsync(resource, expiry, cancellationToken);

				if (acquiredLock != null) return acquiredLock;

				await Task.Delay(retry, cancellationToken);
			}

			return null;
		}
	}
}