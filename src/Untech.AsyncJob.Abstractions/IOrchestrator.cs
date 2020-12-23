using System;
using System.Collections.Generic;
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
		Task StartAsync();

		/// <summary>
		/// Stops this service and waits for completion of all requests without cancellation.
		/// </summary>
		/// <returns>Task to await.</returns>
		Task StopAsync();

		/// <summary>
		/// Stops this service and waits for completion of all requests with cancellation after <paramref name="delay"/>.
		/// Will try to cancel all requests immediately when <paramref name="delay"/> is less or equal <see cref="TimeSpan.Zero"/>.
		/// </summary>
		/// <param name="delay">The delay to wait before requests cancellation.</param>
		/// <returns>Task to await.</returns>
		Task StopAsync(TimeSpan delay);
	}
}
