using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncJob.Metadata.Annotations;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.Throttling
{
	internal class ThrottleMiddleware : IRequestProcessorMiddleware
	{
		private const string AllGroupKey = "*";

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
			var semaphores = GetSemaphoresOrdered(context).ToArray();

			if (semaphores.Length == 0)
			{
				await next(context);
			}
			else
			{
				foreach (var semaphore in semaphores) await semaphore.WaitAsync();

				try
				{
					await next(context);
				}
				finally
				{
					foreach (var semaphore in semaphores.Reverse()) semaphore.Release();
				}
			}
		}

		private IEnumerable<SemaphoreSlim> GetSemaphoresOrdered(Context context)
		{
			var groupKeys = GetGroupKeysOrdered(context);
			foreach (var groupKey in groupKeys)
			{
				var semaphore = TryGetOrAddSemaphore(context, groupKey);
				if (semaphore != null) yield return semaphore;
			}
		}

		// await for request semaphore first and in the end AllGroup semaphore
		// in theory:
		// case: take Request -> Group -> AllGroup semaphore
		// 	awaiting for request/group semaphore won't affect AllGroup semaphore
		// 	so other requests can take allgroup semaphore and do all the things
		// case: take AllGroup -> Group -> Request semaphore
		// 	in that case request can wait for group/request semaphore and waste AllGroup semaphore
		//  so other requests will wait not only for AllGroup but for request/group indirectly
		private IEnumerable<string> GetGroupKeysOrdered(Context context)
		{
			yield return context.RequestName;

			var attr = context.RequestMetadata.GetAttribute<ThrottleGroupAttribute>();
			if (attr != null)
				yield return attr.Group ?? string.Empty;

			yield return AllGroupKey;
		}

		private SemaphoreSlim TryGetOrAddSemaphore(Context context, string groupKey)
		{
			if (_semaphores.TryGetValue(groupKey, out var semaphore)) return semaphore;

			var maxCount = GetRunAtOnce(context, groupKey);
			if (maxCount == null) return null;
			if (maxCount < 1 || maxCount > 100) throw RunAtOnceInvalidError(maxCount);

			lock (_semaphoresWriteSyncRoot)
			{
				if (_semaphores.TryGetValue(groupKey, out semaphore)) return semaphore;

				semaphore = new SemaphoreSlim(maxCount.Value, maxCount.Value);
				_semaphores.Add(groupKey, semaphore);

				return semaphore;
			}
		}

		private static InvalidOperationException RunAtOnceInvalidError(int? maxCount)
		{
			return new InvalidOperationException($"Run At Once is out of range: {maxCount}");
		}

		private int? GetRunAtOnce(Context context, string groupKey)
		{
			if (groupKey == AllGroupKey) return _options.RunAtOnce;

			if (groupKey != context.RequestName) return GetRunAtOnceForGroupFromOptions(groupKey);

			var attr = context.RequestMetadata.GetAttribute<ThrottleAttribute>();
			return attr?.RunAtOnce ?? GetRunAtOnceForGroupFromOptions(groupKey);
		}

		private int? GetRunAtOnceForGroupFromOptions(string groupKey)
		{
			return _options.RunAtOncePerGroups != null && _options.RunAtOncePerGroups.TryGetValue(groupKey, out var runAtOnce)
				? runAtOnce
				: _options.DefaultRunAtOnceInGroup;
		}
	}
}
