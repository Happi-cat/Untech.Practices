using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal interface IHandlerRunner
	{
		object Handle(object args);

		Task HandleAsync(object args, CancellationToken cancellationToken);
	}
}