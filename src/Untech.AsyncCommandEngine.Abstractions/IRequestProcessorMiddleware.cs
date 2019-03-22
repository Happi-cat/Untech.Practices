using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	public interface IRequestProcessorMiddleware
	{
		Task InvokeAsync(Context context, RequestProcessorCallback next);
	}
}