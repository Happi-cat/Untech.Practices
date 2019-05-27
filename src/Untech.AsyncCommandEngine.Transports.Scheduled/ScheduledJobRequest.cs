using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Untech.AsyncCommandEngine.Transports.Scheduled
{
	internal class ScheduledJobRequest : Request
	{
		public ScheduledJobRequest(ScheduledJob job)
		{
			Job = job ?? throw new ArgumentNullException(nameof(job));
			Definition = job.Definition ?? throw new ArgumentNullException(nameof(job.Definition));
			Name = job.Definition.Name ?? throw new ArgumentNullException(nameof(job.Definition.Name));

			Identifier = job.Id + ":" + job.Definition.Name + ":" + job.NextRun;
			Created = job.NextRun ?? DateTime.UtcNow;
			Attributes = Definition.Attributes ?? new Dictionary<string, string>();
		}

		public ScheduledJob Job { get; }
		public ScheduledJobDefinition Definition { get; }

		public override string Identifier { get; }
		public override string Name { get; }
		public override DateTimeOffset Created { get; }
		public override IDictionary<string, string> Attributes { get; }

		public override object GetBody(Type requestType)
		{
			return Definition.GetBody(requestType);
		}

		public override Stream GetRawBody()
		{
			return Definition.GetRawBody();
		}

		public override IEnumerable<Attribute> GetAttachedMetadata()
		{
			return Definition.Metadata ?? base.GetAttachedMetadata();
		}
	}
}