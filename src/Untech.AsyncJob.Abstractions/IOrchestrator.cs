using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Untech.AsyncJob
{
	/// <summary>
	/// Represents interfaces that defines methods that can be used for service management.
	/// </summary>
	public interface IOrchestrator : IHealthCheck
	{
		/// <summary>
		/// Starts this service.
		/// </summary>
		/// <returns>Task to await</returns>
		Task StartAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Stops this service and waits for completion of all requests without cancellation.
		/// </summary>
		/// <returns>Task to await.</returns>
		Task StopAsync(TimeSpan delay, CancellationToken cancellationToken = default);
	}
}
