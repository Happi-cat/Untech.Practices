using System;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Processing
{
	public sealed class AdHocRequestProcessorMiddleware : IRequestProcessorMiddleware
	{
		private readonly Func<Context,RequestProcessorCallback, Task> _middleware;

		public AdHocRequestProcessorMiddleware(Func<Context, RequestProcessorCallback, Task> middleware)
		{
			_middleware = middleware;
		}

		public Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			return _middleware(context, next);
		}
	}
}