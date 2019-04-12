using System;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Processing
{
	public sealed class AdHocRequestProcessorMiddleware : IRequestProcessorMiddleware
	{
		private readonly Func<Context,RequestProcessorCallback, Task> _func;

		public AdHocRequestProcessorMiddleware(Func<Context, RequestProcessorCallback, Task> func)
		{
			_func = func ?? throw new ArgumentNullException(nameof(func));
		}

		public Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			return _func(context, next);
		}
	}
}