using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommmandEngine.Abstractions;

namespace AsyncCommandEngine.Examples.Features.Throttling
{
	internal class ThrottleMiddleware : IAceProcessorMiddleware
	{
		private readonly ThrottleOptions _options;
		private readonly Dictionary<string, SemaphoreSlim> _semaphores;
		private readonly object _semaphoresSyncRoot = new object();

		public ThrottleMiddleware(ThrottleOptions options)
		{
			_options = options;
			_semaphores = new Dictionary<string, SemaphoreSlim>();
		}

		public async Task Execute(AceContext context, AceRequestProcessorDelegate next)
		{
			var semaphores = GetSemaphores(GetGroupKeys(context.Request.Metadata));

			await Task.WhenAll(semaphores.Select(s => s.WaitAsync(context.RequestAborted)));

			try
			{
				await next(context);
			}
			finally
			{
				semaphores.ForEach(s => s.Release());
			}
		}

		private IEnumerable<string> GetGroupKeys(IRequestMetadata metadata)
		{
			yield return "*";

			foreach (var attribute in metadata.GetAttributes<ThrottleGroupAttribute>())
			{
				if (!string.IsNullOrEmpty(attribute.Group))
					yield return attribute.Group;
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

			lock (_semaphoresSyncRoot)
			{
				if (_semaphores.TryGetValue(groupKey, out semaphore)) return semaphore;

				semaphore = new SemaphoreSlim(0, maxCount.Value);
				_semaphores.Add(groupKey, semaphore);

				return semaphore;
			}
		}

		private int? GetMaxCount(string groupKey)
		{
			if (groupKey == "*") return _options.DefaultRunAtOnce;

			if (_options.Groups.TryGetValue(groupKey, out var options))
			{
				return options.RunAtOnce ?? _options.DefaultRunAtOnceInGroup;
			}

			return _options.DefaultRunAtOnceInGroup;
		}
	}
}