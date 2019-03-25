using System;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	public interface IOrchestrator
	{
		Task StartAsync();
		Task StopAsync(TimeSpan delay);
	}
}