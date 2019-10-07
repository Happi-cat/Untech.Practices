using System;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.AsyncJob.Features.Debounce
{
	/// <summary>
	/// Represents interfaces of a data storage that contains datetime of the request last run.
	/// </summary>
	public interface ILastRunStore
	{
		/// <summary>
		/// Gets the <paramref name="request"/> last run time.
		/// </summary>
		/// <param name="request">The request to get last run datetime.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Last run datetime or null.</returns>
		Task<DateTimeOffset> GetLastRunAsync(Request request, CancellationToken cancellationToken);

		/// <summary>
		/// Saves last run datetime of the <paramref name="request"/>.
		/// </summary>
		/// <param name="request">The request to save.</param>
		/// <returns>Task to await.</returns>
		Task SetLastRunAsync(Request request);
	}
}
