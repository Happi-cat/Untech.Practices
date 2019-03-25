using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Processing
{
	public interface IRequestProcessorMiddleware
	{
		Task InvokeAsync(Context context, RequestProcessorCallback next);
	}
}