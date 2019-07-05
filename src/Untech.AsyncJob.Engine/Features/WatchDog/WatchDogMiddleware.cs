using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob.Metadata;
using Untech.AsyncJob.Metadata.Annotations;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.WatchDog
{
	internal class WatchDogMiddleware : IRequestProcessorMiddleware
	{
		private readonly WatchDogOptions _options;
		private readonly ILogger _logger;

		public WatchDogMiddleware(WatchDogOptions options, ILoggerFactory loggerFactory)
		{
			_options = options;
			_logger = loggerFactory.CreateLogger<WatchDogMiddleware>();
		}

		public async Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			var timeout = GetTimeout(context);
			if (timeout != null && timeout > TimeSpan.Zero)
			{
				var watchdogTokenSource = new CancellationTokenSource(timeout.Value);
				var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
					watchdogTokenSource.Token,
					context.Aborted);

				context.Aborted = linkedTokenSource.Token;
				_logger.TimeoutConfigured(context, timeout.Value);
			}

			await next(context);
		}

		private TimeSpan? GetTimeout(Context context)
		{
			var attr = context.RequestMetadata.GetAttribute<WatchDogTimeoutAttribute>();

			if (attr != null) return attr.Timeout;
			if (_options.TimeoutPerRequests != null
				&& _options.TimeoutPerRequests.TryGetValue(context.RequestName, out var timeout))
				return timeout;
			return _options.DefaultTimeout;
		}
	}
}
