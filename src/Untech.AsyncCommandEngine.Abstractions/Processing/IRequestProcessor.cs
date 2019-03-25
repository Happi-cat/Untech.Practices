using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Processing
{
	public interface IRequestProcessor
	{
		Task InvokeAsync(Context context);
	}
}