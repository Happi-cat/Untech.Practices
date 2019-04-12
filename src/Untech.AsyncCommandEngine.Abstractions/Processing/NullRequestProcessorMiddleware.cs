using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Processing
{
	public sealed class NullRequestProcessorMiddleware : IRequestProcessorMiddleware
	{
		public static readonly IRequestProcessorMiddleware Instance = new NullRequestProcessorMiddleware();

		public Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			return Task.FromResult(0);
		}
	}
}