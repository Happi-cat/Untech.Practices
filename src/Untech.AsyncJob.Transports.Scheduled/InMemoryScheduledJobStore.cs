using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Untech.AsyncJob.Transports.Scheduled
{
	public class InMemoryScheduledJobStore : IScheduledJobStore
	{
		private readonly IReadOnlyDictionary<string, ScheduledJobDefinition> _jobDefinitions;
		private readonly ConcurrentDictionary<string, DateTimeOffset> _nextRuns;

		public InMemoryScheduledJobStore(IEnumerable<ScheduledJobDefinition> jobDefinitions)
		{
			_jobDefinitions = jobDefinitions
				.Select((n, i) => (item: n, id: i))
				.ToDictionary(n => n.id.ToString(), n => n.item);
			_nextRuns = new ConcurrentDictionary<string, DateTimeOffset>();
		}

		public Task<IEnumerable<ScheduledJob>> GetJobsAsync()
		{
			return Task.FromResult<IEnumerable<ScheduledJob>>(
				_jobDefinitions
					.Select(jd => new ScheduledJob(jd.Key, jd.Value, GetNextRun(jd.Key)))
					.ToList()
			);
		}

		public Task SaveNextRun(ScheduledJob job, DateTimeOffset nextRun)
		{
			_nextRuns.AddOrUpdate(job.Id, nextRun, (s, time) => nextRun);

			return Task.CompletedTask;
		}

		public Task TrackResult(ScheduledJob job, Exception exception)
		{
			return Task.CompletedTask;
		}

		private DateTimeOffset GetNextRun(string id)
		{
			return _nextRuns.GetOrAdd(id, DateTimeOffset.UtcNow);
		}
	}
}
