using System;
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

		public async Task<Request[]> GetRequestsAsync(int count)
		{
			var jobs = await _scheduledJobStore.GetJobsAsync();
			var jobsToRun = jobs
				.Where(j => j.CanRunNow())
				.OrderBy(j => j.NextRun)
				.Take(count)
				.Select(Create)
				.ToArray();

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

		public Task FlushAsync()
		{
			return Task.CompletedTask;
		}

		private async Task Complete(Request request, Exception exception = null)
		{
			if (request.Items.TryGetValue(typeof(ScheduledJob), out var obj) && obj is ScheduledJob job)
			{
				await _scheduledJobStore.SaveNextRun(job, job.GetNewNextRun());
				await _scheduledJobStore.TrackResult(job, exception);
			}
		}

		private static Request Create(ScheduledJob job)
		{
			return new ScheduledJobRequest(job);
		}
	}
}
