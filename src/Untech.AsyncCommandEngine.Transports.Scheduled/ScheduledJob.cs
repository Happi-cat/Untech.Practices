using System;
using System.Runtime.Serialization;

namespace Untech.AsyncCommandEngine.Transports.Scheduled
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
			if (!IsAlive()) return false;

			return NextRun == null || NextRun <= DateTime.UtcNow;
		}

		public DateTimeOffset GetNewNextRun()
		{
			var now = DateTime.UtcNow;
			var nextRun = NextRun ?? now;

			if (!IsAlive()) return nextRun;

			while (nextRun <= now) nextRun += Definition.Interval;
			return nextRun;
		}

		private bool IsAlive()
		{
			if (Disabled) return false;

			var now = DateTime.UtcNow;

			if (now < StartAfter) return false;
			if (StopAfter < now) return false;
			return true;
		}
	}
}