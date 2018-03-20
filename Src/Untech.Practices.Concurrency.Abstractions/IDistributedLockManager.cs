using System;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Concurrency.Abstractions
{
	public interface IDistributedLockManager
	{
		IDisposable Acquire(string resource, AcquireOptions options);

		Task<IDisposable> AcquireAsync(string resource, AcquireOptions options,
			CancellationToken cancellationToken = default(CancellationToken));

		IDisposable TryAcquire(string resource, AcquireOptions options);

		Task<IDisposable> TryAcquireAsync(string resource, AcquireOptions options,
			CancellationToken cancellationToken = default(CancellationToken));
	}
}