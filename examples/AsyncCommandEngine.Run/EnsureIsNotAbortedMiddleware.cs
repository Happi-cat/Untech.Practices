using System.Threading.Tasks;
using Untech.AsyncCommmandEngine.Abstractions;

namespace AsyncCommandEngine.Run
{
	internal class EnsureIsNotAbortedMiddleware : IAceProcessorMiddleware
	{
		public Task Execute(AceContext context, AceRequestProcessorDelegate next)
		{
			context.RequestAborted.ThrowIfCancellationRequested();

			return next(context);
		}
	}
}