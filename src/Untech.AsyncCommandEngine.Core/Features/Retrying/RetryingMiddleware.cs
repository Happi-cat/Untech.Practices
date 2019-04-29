using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine.Features.Retrying
{
	internal class RetryingMiddleware : IRequestProcessorMiddleware
	{
		private readonly IRetryPolicy _retryPolicy;
		private readonly ILogger _logger;

		public RetryingMiddleware(IRetryPolicy retryPolicy, ILoggerFactory loggerFactory)
		{
			_retryPolicy = retryPolicy;
			_logger = loggerFactory.CreateLogger<RetryingMiddleware>();
		}
		public async Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			bool retry;
			int remainingAttempts = _retryPolicy.RetryCount;

			do
			{
				retry = false;
				try { await next(context); }
				catch (Exception e)
				{
					retry = _retryPolicy.RetryOnError(e) && remainingAttempts-- > 0;

					if (retry)
					{
						_logger.WillRetry(context, _retryPolicy.RetryCount - remainingAttempts, e);
						await Task.Delay(_retryPolicy.GetSleepDuration(e));
					}
					else throw;
				}
			} while (retry);
		}
	}
}