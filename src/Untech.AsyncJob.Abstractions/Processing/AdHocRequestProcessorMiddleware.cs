using System;
using System.Threading.Tasks;

namespace Untech.AsyncJob.Processing
{
	/// <summary>
	/// Represents <see cref="IRequestProcessorMiddleware"/> that decorates <see cref="Func{}"/>.
	/// </summary>
	public sealed class AdHocRequestProcessorMiddleware : IRequestProcessorMiddleware
	{
		private readonly Func<Context, RequestProcessorCallback, Task> _func;

		/// <summary>
		/// Initializes a new instance of the <see cref="AdHocRequestProcessorMiddleware"/>
		/// with the specified <paramref name="func"/> to be invoked.
		/// </summary>
		/// <param name="func">The func to be invoked as a part of pipeline.</param>
		public AdHocRequestProcessorMiddleware(Func<Context, RequestProcessorCallback, Task> func)
		{
			_func = func ?? throw new ArgumentNullException(nameof(func));
		}

		/// <inheritdoc />
		public Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			return _func(context, next);
		}
	}
}
