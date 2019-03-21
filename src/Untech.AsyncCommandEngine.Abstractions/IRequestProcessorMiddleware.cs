using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	public interface IRequestProcessorMiddleware
	{
		Task ExecuteAsync(Context context, RequestProcessorCallback next);
	}
}