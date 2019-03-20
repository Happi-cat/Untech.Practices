using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	internal class EnsureIsNotAbortedMiddleware : IAceProcessorMiddleware
	{
		public Task ExecuteAsync(AceContext context, AceRequestProcessorDelegate next)
		{
			context.RequestAborted.ThrowIfCancellationRequested();

			return next(context);
		}
	}
}