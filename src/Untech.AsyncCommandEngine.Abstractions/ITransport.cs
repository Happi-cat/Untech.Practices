using System;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	public interface IAppBuilder
	{
		IAppBuilder UseTransport(ITransport transport);
		IAppBuilder Use(Func<IRequestProcessorMiddleware> processorMiddleware);
		IOrchestrator Build();
	}

	public interface ITransport
	{
		Task<Request> GetRequestAsync();
		Task CompleteRequestAsync(Request request);
		Task FailRequestAsync(Request request, Exception exception);
	}

	public interface IOrchestrator
	{
		Task StartAsync();
		Task StopAsync(CancellationToken cancellationToken);
	}
}