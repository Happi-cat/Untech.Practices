using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommmandEngine.Abstractions;

namespace AsyncCommandEngine.Run
{
	public class WatchDogProcessorMiddleware : IAceProcessorMiddleware
	{
		private readonly WatchDogOptions _options;

		public WatchDogProcessorMiddleware(WatchDogOptions options)
		{
			_options = options;
		}

		public async Task Execute(AceContext context, AceRequestProcessorDelegate next)
		{
			var timeout = GetTimeout(context);
			if (timeout != null)
			{
				var watchdogTokenSource = new CancellationTokenSource(timeout.Value);
				var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
					watchdogTokenSource.Token,
					context.RequestAborted);

				context.RequestAborted = linkedTokenSource.Token;
			}

			await next(context);
		}

		private TimeSpan? GetTimeout(AceContext context)
		{
			var attr = context.Request.Metadata.GetAttribute<WatchDogTimeoutAttribute>();

			if (attr != null) return attr.Timeout;
			if (_options.RequestTimeouts.TryGetValue(context.Request.TypeName, out var timeout)) return timeout;
			return _options.DefaultTimeout;
		}
	}
}