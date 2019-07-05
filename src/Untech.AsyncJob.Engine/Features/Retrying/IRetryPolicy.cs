using System;

namespace Untech.AsyncJob.Features.Retrying
{
	public interface IRetryPolicy
	{
		int RetryCount { get; }

		TimeSpan GetSleepDuration(int attempt, Exception e);

		bool RetryOnError(int attempt, Exception e);
	}
}
