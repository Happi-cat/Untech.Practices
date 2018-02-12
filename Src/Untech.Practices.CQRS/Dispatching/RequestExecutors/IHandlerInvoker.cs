using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal interface IHandlerInvoker
	{
		object Invoke(object args);

		Task InvokeAsync(object args, CancellationToken cancellationToken);
	}
}