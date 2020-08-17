using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Untech.AsyncJob.Formatting.Json;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncJob.Transports.InProcess
{
	internal class InProcessRequest : ObjectRequest
	{
		private readonly int _priority;
		private readonly TimeSpan? _expiresAfter;
		private readonly TimeSpan? _executeAfter;

		public InProcessRequest([NotNull] object payload, [CanBeNull] QueueOptions options)
			: base(payload, JsonRequestContentFormatter.Default, GetAttributes(options))
		{
			_priority = options?.Priority ?? 0;
			_expiresAfter = options?.ExpiresAfter;
			_executeAfter = options?.ExecuteAfter;
		}

		public int GetPriority()
		{
			return _priority;
		}

		public bool IsExpired()
		{
			return _expiresAfter != null && Created + _expiresAfter <= DateTimeOffset.Now;
		}

		public bool IsExecuteAfterReached()
		{
			return _executeAfter == null || Created + _executeAfter <= DateTimeOffset.Now;
		}

		private static IReadOnlyDictionary<string, string> GetAttributes([CanBeNull] QueueOptions options)
		{
			return options?.Advanced?
				.ToDictionary(n => n.Key, n => Convert.ToString(n.Value));
		}
	}
}