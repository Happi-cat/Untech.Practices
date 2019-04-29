using System;

namespace Untech.AsyncCommandEngine.Features.Retrying
{
	public interface IRetryPolicy
	{
		int RetryCount { get; }

		TimeSpan GetSleepDuration(Exception e);

		bool RetryOnError(Exception e);
	}
}