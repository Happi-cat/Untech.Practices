using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal interface IRequestExecutor
	{
		object Handle(object args);

		Task HandleAsync(object args);
	}
}