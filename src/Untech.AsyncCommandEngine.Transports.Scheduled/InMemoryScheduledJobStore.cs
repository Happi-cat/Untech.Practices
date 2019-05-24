using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Transports.Scheduled
{
	public class InMemoryScheduledJobStore : IScheduledJobStore
	{
		private readonly IReadOnlyCollection<ScheduledJobDefinition> _jobDefinitions;
		private readonly ConcurrentDictionary<string, DateTime> _nextRuns;

		public InMemoryScheduledJobStore(IEnumerable<ScheduledJobDefinition> jobDefinitions)
		{
			_jobDefinitions = jobDefinitions.ToList();
			_nextRuns = new ConcurrentDictionary<string, DateTime>();
		}

		public Task<IEnumerable<ScheduledJob>> GetJobsAsync()
		{
			return Task.FromResult<IEnumerable<ScheduledJob>>(
				_jobDefinitions
					.Select(jd => new ScheduledJob(jd, GetNextRun(jd)))
					.ToList()
			);
		}

		public Task SaveNextRun(ScheduledJob job, DateTime nextRun)
		{
			_nextRuns.AddOrUpdate(job.Definition.Name, nextRun, (s, time) => nextRun);

			return Task.CompletedTask;
		}

		public Task TrackResult(ScheduledJob job, Exception exception)
		{
			return Task.CompletedTask;
		}

		private DateTime GetNextRun(ScheduledJobDefinition jobDefinition)
		{
			return _nextRuns.GetOrAdd(jobDefinition.Name, DateTime.UtcNow);
		}
	}
}