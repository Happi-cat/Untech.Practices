using System;
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
			var semaphores = GetSemaphores(GetGroupKeys(context));

			await WaitAsync(semaphores, context);

			try
			{
				await next(context);
			}
			finally
			{
				Release(semaphores);
			}
		}

		private static Task WaitAsync(IEnumerable<SemaphoreSlim> semaphores, Context context)
		{
			var waitTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
			var compositeTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
				waitTokenSource.Token,
				context.Aborted);

			return WaitAsync(semaphores, compositeTokenSource.Token);
		}

		private static Task WaitAsync(IEnumerable<SemaphoreSlim> semaphores, CancellationToken cancellationToken)
		{
			return Task.WhenAll(semaphores.Select(s => s.WaitAsync(cancellationToken)));
		}

		private static void Release(IEnumerable<SemaphoreSlim> semaphores)
		{
			foreach (var semaphore in semaphores) semaphore.Release();
		}

		private IEnumerable<string> GetGroupKeys(Context context)
		{
			return GetEnumerable()
				.Where(n => !string.IsNullOrEmpty(n))
				.Distinct()
				.OrderBy(n => n);

			IEnumerable<string> GetEnumerable()
			{
				foreach (var attribute in context.RequestMetadata.GetAttributes<ThrottleGroupAttribute>())
					yield return attribute.Group;

				yield return context.RequestName;
			}
		}

		private List<SemaphoreSlim> GetSemaphores(IEnumerable<string> groups)
		{
			return GetEnumerable().Where(n => n != null).ToList();

			IEnumerable<SemaphoreSlim> GetEnumerable()
			{
				foreach (var @group in groups)
				{
					yield return TryGetOrAddSemaphore(group);
				}
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