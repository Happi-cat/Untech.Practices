using System;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Features.Debounce
{
	public interface ILastRunStore
	{
		Task<DateTimeOffset?> GetLastRunAsync(Request request, CancellationToken cancellationToken);
		Task SetLastRunAsync(Request request);
	}
}