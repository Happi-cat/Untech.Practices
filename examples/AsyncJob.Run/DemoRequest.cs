using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Untech.AsyncJob;
using Untech.AsyncJob.Formatting.Json;
using Untech.AsyncJob.Metadata.Annotations;

namespace AsyncJob.Run
{
	internal class DemoRequest : ObjectRequest
	{
		public DemoRequest(string id, object body) : base(id, DateTimeOffset.Now, body, JsonRequestContentFormatter.Default)
		{
		}

		public ReadOnlyCollection<MetadataAttribute> AttachedMetadata { get; set; }

		public override IEnumerable<MetadataAttribute> GetAttachedMetadata()
		{
			return AttachedMetadata ?? base.GetAttachedMetadata();
		}
	}
}
