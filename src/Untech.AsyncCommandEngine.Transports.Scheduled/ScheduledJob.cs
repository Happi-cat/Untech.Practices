using System;

namespace Untech.AsyncCommandEngine.Transports.Scheduled
{
	public class ScheduledJob
	{
		private ScheduledJob()
		{
		}

		public ScheduledJob(string id, ScheduledJobDefinition definition, DateTime? nextRun = null)
		{
			Id = id;
			Definition = definition;
			NextRun = nextRun;
		}

		public string Id { get; private set; }

		public ScheduledJobDefinition Definition { get; private set; }

		public DateTime? NextRun { get; private set; }

		public DateTime? StartAfter { get; set; }

		public DateTime? StopAfter { get; set; }

		public bool Disabled { get; set; }

		public bool IsEnabled()
		{
			if (Disabled) return false;

			var now = DateTime.UtcNow;

			if (now < StartAfter) return false;
			if (StopAfter < now) return false;
			return true;
		}

		public bool CanRunNow()
		{
			if (!IsEnabled()) return false;

			return NextRun == null || NextRun <= DateTime.UtcNow;
		}

		public DateTime GetNewNextRun()
		{
			var now = DateTime.UtcNow;
			var nextRun = NextRun ?? now;

			if (!IsEnabled()) return nextRun;

			while (nextRun <= now) nextRun += Definition.Interval;
			return nextRun;
		}
	}
}