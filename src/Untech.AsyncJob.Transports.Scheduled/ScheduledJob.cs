using System;
using System.Runtime.Serialization;
using NCrontab;

namespace Untech.AsyncJob.Transports.Scheduled
{
	[DataContract]
	public class ScheduledJob
	{
		private ScheduledJob()
		{
		}

		public ScheduledJob(string id, ScheduledJobDefinition definition, DateTimeOffset? nextRun = null)
		{
			Id = id;
			Definition = definition;
			NextRun = nextRun;
		}

		[DataMember]
		public string Id { get; private set; }

		[DataMember]
		public ScheduledJobDefinition Definition { get; private set; }

		[DataMember]
		public DateTimeOffset? NextRun { get; private set; }

		[DataMember]
		public DateTimeOffset? StartAfter { get; set; }

		[DataMember]
		public DateTimeOffset? StopAfter { get; set; }

		[DataMember]
		public bool Disabled { get; set; }

		public bool CanRunNow()
		{
			if (!IsAlive())
				return false;

			return NextRun == null || NextRun <= DateTime.UtcNow;
		}

		public DateTimeOffset GetNewNextRun()
		{
			var now = DateTime.UtcNow;
			var currentRun = NextRun ?? now;

			if (!IsAlive())
				return currentRun;

			return CrontabSchedule
				.Parse(Definition.Cron)
				.GetNextOccurrence(currentRun.UtcDateTime);
		}

		private bool IsAlive()
		{
			if (Disabled)
				return false;

			var now = DateTime.UtcNow;

			return !(now < StartAfter || StopAfter < now);
		}
	}
}
