using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine
{
	/// <summary>
	/// Represents interfaces that defines methods for work with requests store.
	/// </summary>
	public interface ITransport
	{
		/// <summary>
		/// Retrieves requests from store.
		/// </summary>
		/// <param name="count">The amount of requests to retrieve.</param>
		/// <returns></returns>
		Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count);

		/// <summary>
		/// Marks the specified <paramref name="request"/> as a completed in the current store.
		/// </summary>
		/// <param name="request">The request that was completed without any errors.</param>
		/// <returns>Task to await.</returns>
		Task CompleteRequestAsync(Request request);

		/// <summary>
		/// Marks the specified <paramref name="request"/> as a failed in the current store.
		/// </summary>
		/// <param name="request">The request that failed.</param>
		/// <param name="exception">The exception that was thrown during request processing.</param>
		/// <returns>Task to await.</returns>
		Task FailRequestAsync(Request request, Exception exception);
	}
}