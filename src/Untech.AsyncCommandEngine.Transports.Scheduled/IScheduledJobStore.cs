using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Transports.Scheduled
{
	public interface IScheduledJobStore
	{
		Task<IEnumerable<ScheduledJob>> GetJobsAsync();

		Task SaveNextRun(ScheduledJob job, DateTimeOffset nextRun);

		Task TrackResult(ScheduledJob job, Exception exception);
	}
}