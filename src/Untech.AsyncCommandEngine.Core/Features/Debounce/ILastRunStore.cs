using System;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Features.Debounce
{
	public interface ILastRunStore
	{
		Task<DateTimeOffset?> GetLastRunAsync(AceRequest request, CancellationToken cancellationToken);
		Task SetLastRunAsync(AceRequest request);
	}
}