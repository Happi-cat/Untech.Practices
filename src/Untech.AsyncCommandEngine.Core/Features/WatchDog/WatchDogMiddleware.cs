using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine.Features.WatchDog
{
	internal class WatchDogMiddleware : IRequestProcessorMiddleware
	{
		private readonly WatchDogOptions _options;

		public WatchDogMiddleware(WatchDogOptions options)
		{
			_options = options;
		}

		public async Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			var timeout = GetTimeout(context);
			if (timeout != null)
			{
				var watchdogTokenSource = new CancellationTokenSource(timeout.Value);
				var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
					watchdogTokenSource.Token,
					context.Aborted);

				context.Aborted = linkedTokenSource.Token;
			}

			await next(context);
		}

		private TimeSpan? GetTimeout(Context context)
		{
			var attr = context.RequestMetadata.GetAttribute<WatchDogTimeoutAttribute>();

			if (attr != null) return attr.Timeout;
			if (_options.RequestTimeouts != null
				&& _options.RequestTimeouts.TryGetValue(context.RequestName, out var timeout))
				return timeout;
			return _options.DefaultTimeout;
		}
	}
}