using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncJob.Features.Retrying
{
	public class RetryPolicy : IRetryPolicy
	{
		private readonly int _retryCount;
		private readonly TimeSpan _sleepDuration;
		private readonly IReadOnlyCollection<Type> _exceptions;

		public RetryPolicy(IEnumerable<Type> exceptions)
			: this(1, exceptions)
		{
		}

		public RetryPolicy(TimeSpan sleepDuration, IEnumerable<Type> exceptions)
			: this(1, sleepDuration, exceptions)
		{
		}

		public RetryPolicy(int retryCount, IEnumerable<Type> exceptions)
			: this(retryCount, TimeSpan.Zero, exceptions)
		{
		}

		public RetryPolicy(int retryCount, TimeSpan sleepDuration, IEnumerable<Type> exceptions)
		{
			if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount));
			if (sleepDuration < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(sleepDuration));
			if (exceptions == null) throw new ArgumentNullException(nameof(exceptions));

			_retryCount = retryCount;
			_sleepDuration = sleepDuration;
			_exceptions = exceptions.ToList();
		}

		public int RetryCount => _retryCount;

		public TimeSpan GetSleepDuration(int attempt, Exception e)
		{
			return _sleepDuration;
		}

		public bool RetryOnError(int attempt, Exception e)
		{
			var type = e.GetType();

			return _exceptions.Any(n => n == type)
				|| _exceptions.Any(n => n.IsAssignableFrom(type));
		}
	}
}
