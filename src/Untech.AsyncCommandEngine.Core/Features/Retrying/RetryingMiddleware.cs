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
			int attempt = 0;

			do
			{
				retry = false;

				try { await next(context); }
				catch (Exception e)
				{
					retry = _retryPolicy.RetryOnError(attempt, e) && attempt < _retryPolicy.RetryCount;

					if (retry)
					{
						_logger.WillRetry(context, attempt, e);
						await Task.Delay(_retryPolicy.GetSleepDuration(attempt, e));
					}
					else throw;
				}

				attempt++;
			} while (retry);
		}
	}
}