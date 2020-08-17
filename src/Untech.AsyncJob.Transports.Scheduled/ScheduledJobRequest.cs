using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Untech.AsyncJob.Metadata.Annotations;

namespace Untech.AsyncJob.Transports.Scheduled
{
	internal class ScheduledJobRequest : Request
	{
		private readonly DateTimeOffset _instantiated = DateTimeOffset.Now;
		private readonly ScheduledJob _job;
		private readonly ScheduledJobDefinition _definition;

		public ScheduledJobRequest([NotNull] ScheduledJob job)
		{
			_job = job ?? throw new ArgumentNullException(nameof(job));
			_definition = job.Definition ?? throw new ArgumentNullException(nameof(job.Definition));

			Items[typeof(ScheduledJob)] = job;
		}

		public override string Identifier => $"{_job.Id}:{Name}:{Created}";
		public override string Name => _definition.Name;
		public override DateTimeOffset Created => _job.NextRun ?? _instantiated;

		public override IReadOnlyDictionary<string, string> Attributes => _definition.Attributes;
		public override string Content => _definition.Content;
		public override string ContentType => _definition.ContentType;

		public override IEnumerable<MetadataAttribute> GetAttachedMetadata()
		{
			return _definition.Metadata ?? base.GetAttachedMetadata();
		}
	}
}
