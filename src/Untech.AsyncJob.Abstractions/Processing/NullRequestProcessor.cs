using System.Threading.Tasks;

namespace Untech.AsyncJob.Processing
{
	public sealed class NullRequestProcessor : IRequestProcessor
	{
		public static readonly IRequestProcessor Instance = new NullRequestProcessor();

		public Task InvokeAsync(Context context)
		{
			return Task.CompletedTask;
		}
	}
}