using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.Retrying
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
			int attempt = 0;

			do
			{
				try
				{
					await next(context);
					break;
				}
				catch (TaskCanceledException) { throw; }
				catch (OperationCanceledException) { throw; }
				catch (Exception e)
				{
					if (attempt++ >= _retryPolicy.RetryCount)
						throw;
					if (!_retryPolicy.RetryOnError(attempt, e))
						throw;

					_logger.WillRetry(context, attempt, e);
					await Task.Delay(_retryPolicy.GetSleepDuration(attempt, e));
				}
			} while (true);
		}
	}
}
