using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommmandEngine.Abstractions;

namespace AsyncCommandEngine.Examples.Features.Debounce
{
	public interface ILastRunStore
	{
		Task<DateTimeOffset?> GetLastRunAsync(AceRequest request, CancellationToken cancellationToken);
		Task SetLastRunAsync(AceRequest request);
	}
}