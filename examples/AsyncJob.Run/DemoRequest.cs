using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using Untech.AsyncJob;
using Untech.AsyncJob.Metadata.Annotations;

namespace AsyncJob.Run
{
	internal class DemoRequest : Request
	{
		private readonly object _body;

		public DemoRequest(string id, object body)
		{
			Identifier = id;

			_body = body;

			Name = body.GetType().FullName;
			Created = DateTimeOffset.UtcNow;
		}

		public override string Identifier { get; }
		public override string Name { get; }

		public override DateTimeOffset Created { get; }
		public override IDictionary<string, string> Attributes { get; }

		public ReadOnlyCollection<MetadataAttribute> AttachedMetadata { get; set; }

		public override object GetBody(Type requestType)
		{
			return _body;
		}

		public override Stream GetRawBody()
		{
			var body = JsonSerializer.ToString(_body);
			return new MemoryStream(Encoding.UTF8.GetBytes(body));
		}

		public override IEnumerable<MetadataAttribute> GetAttachedMetadata()
		{
			return AttachedMetadata ?? base.GetAttachedMetadata();
		}
	}
}
