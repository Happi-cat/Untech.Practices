using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine.Features.Throttling
{
	internal class ThrottleMiddleware : IRequestProcessorMiddleware
	{
		private readonly ThrottleOptions _options;
		private readonly Dictionary<string, SemaphoreSlim> _semaphores;
		private readonly object _semaphoresWriteSyncRoot = new object();

		public ThrottleMiddleware(ThrottleOptions options)
		{
			_options = options;
			_semaphores = new Dictionary<string, SemaphoreSlim>();
		}

		public async Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			var semaphore = TryGetOrAddSemaphore(GetGroupKey(context));

			if (semaphore == null)
			{
				await next(context);
			}
			else
			{
				await semaphore.WaitAsync(context.Aborted);

				try
				{
					await next(context);
				}
				finally
				{
					semaphore.Release();
				}
			}
		}

		private string GetGroupKey(Context context)
		{
			return GetEnumerable()
				.Where(n => !string.IsNullOrEmpty(n))
				.OrderBy(n => n)
				.FirstOrDefault();

			IEnumerable<string> GetEnumerable()
			{
				var attr = context.RequestMetadata.GetAttribute<ThrottleGroupAttribute>();
				if (attr != null)
					yield return attr.Group;

				yield return context.RequestName;
			}
		}

		private SemaphoreSlim TryGetOrAddSemaphore(string groupKey)
		{
			if (_semaphores.TryGetValue(groupKey, out var semaphore)) return semaphore;

			var maxCount = GetMaxCount(groupKey);
			if (maxCount == null) return null;

			lock (_semaphoresWriteSyncRoot)
			{
				if (_semaphores.TryGetValue(groupKey, out semaphore)) return semaphore;

				semaphore = new SemaphoreSlim(maxCount.Value, maxCount.Value);
				_semaphores.Add(groupKey, semaphore);

				return semaphore;
			}
		}

		private int? GetMaxCount(string groupKey)
		{
			if (string.IsNullOrEmpty(groupKey)) return null;

			if (_options.Groups != null && _options.Groups.TryGetValue(groupKey, out var options))
				return options.RunAtOnce ?? _options.DefaultRunAtOnceInGroup;

			return _options.DefaultRunAtOnceInGroup;
		}
	}
}