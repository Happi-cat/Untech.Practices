using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Untech.AsyncJob.Transports.Scheduled
{
	public class ScheduledTransport : ITransport
	{
		private readonly IScheduledJobStore _scheduledJobStore;

		public ScheduledTransport(IScheduledJobStore scheduledJobStore)
		{
			_scheduledJobStore = scheduledJobStore;
		}

		public async Task<ReadOnlyCollection<Request>> GetRequestsAsync(int count)
		{
			var jobs = await _scheduledJobStore.GetJobsAsync();
			var jobsToRun = jobs
				.Where(j => j.CanRunNow())
				.OrderBy(j => j.NextRun)
				.Take(count)
				.Select(j => new ScheduledJobRequest(j))
				.ToList<Request>()
				.AsReadOnly();

			return jobsToRun;
		}

		public Task CompleteRequestAsync(Request request)
		{
			return Complete(request);
		}

		public Task FailRequestAsync(Request request, Exception exception)
		{
			return Complete(request, exception);
		}

		private async Task Complete(Request request, Exception exception = null)
		{
			if (!(request is ScheduledJobRequest jobRequest)) return;

			var job = jobRequest.Job;
			await _scheduledJobStore.SaveNextRun(job, job.GetNewNextRun());
			await _scheduledJobStore.TrackResult(job, exception);
		}
	}
}
