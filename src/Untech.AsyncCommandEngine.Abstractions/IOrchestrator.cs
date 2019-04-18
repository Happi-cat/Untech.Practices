using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	public interface IOrchestrator
	{
		Task StartAsync();

		IReadOnlyDictionary<string, object> GetState();

		Task StopAsync();

		Task StopAsync(TimeSpan delay);
	}
}