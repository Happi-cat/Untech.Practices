using System;

namespace Untech.AsyncCommandEngine.Transports.Scheduled
{
	public class ScheduledJob
	{
		private ScheduledJob()
		{
		}

		public ScheduledJob(ScheduledJobDefinition definition, DateTime? nextRun = null)
		{
			Definition = definition;
			NextRun = nextRun;
		}

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

		public void UpdateNextRun()
		{
			if (!IsEnabled()) return;

			var now = DateTime.UtcNow;
			while (NextRun == null || NextRun <= now) NextRun += Definition.Interval;
		}
	}
}