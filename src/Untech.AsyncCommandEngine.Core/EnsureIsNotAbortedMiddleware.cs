using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	internal class EnsureIsNotAbortedMiddleware : IRequestProcessorMiddleware
	{
		public Task ExecuteAsync(Context context, RequestProcessorCallback next)
		{
			context.Aborted.ThrowIfCancellationRequested();

			return next(context);
		}
	}
}