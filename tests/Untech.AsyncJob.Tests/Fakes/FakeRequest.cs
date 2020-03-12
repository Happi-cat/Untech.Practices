using System;
using System.Collections.Generic;
using Untech.AsyncJob.Formatting.Json;
using Untech.AsyncJob.Metadata.Annotations;

namespace Untech.AsyncJob.Fakes
{
	public class FakeRequest : ObjectRequest
	{
		private readonly IEnumerable<MetadataAttribute> _meta;

		public FakeRequest(DateTimeOffset? created = null, object body = null, IEnumerable<MetadataAttribute> meta = null)
			: base(Guid.NewGuid().ToString(), created ?? DateTimeOffset.Now, body ?? new object(), JsonRequestContentFormatter.Default)
		{
			_meta = meta;
		}

		public override IEnumerable<MetadataAttribute> GetAttachedMetadata()
		{
			return _meta ?? base.GetAttachedMetadata();
		}
	}
}
