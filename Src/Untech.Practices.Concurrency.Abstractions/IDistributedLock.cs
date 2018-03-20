using System;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.Concurrency.Abstractions
{
	public interface IDistributedLock
	{
		IDisposable TryAcquire(string resource, TimeSpan expiryTime);

		Task<IDisposable> TryAcquireAsync(string resource, TimeSpan expiryTime,
			CancellationToken cancellationToken = default(CancellationToken));
	}
}