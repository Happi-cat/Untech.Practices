using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching.Processors
{
	internal interface IProcessor
	{
		Task InvokeAsync(object args, CancellationToken cancellationToken);
	}
}