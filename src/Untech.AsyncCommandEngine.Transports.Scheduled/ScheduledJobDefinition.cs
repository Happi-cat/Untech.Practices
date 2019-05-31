using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Untech.AsyncCommandEngine.Metadata.Annotations;

namespace Untech.AsyncCommandEngine.Transports.Scheduled
{
	[DataContract]
	public class ScheduledJobDefinition
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public IDictionary<string, string> Attributes { get; set; }

		[DataMember]
		public IEnumerable<MetadataAttribute> Metadata { get; set; }

		[DataMember]
		public TimeSpan Interval { get; set; }

		[DataMember]
		public string Body { get; set; }

		public virtual object GetBody(Type requestType)
		{
			return System.Text.Json.Serialization.JsonSerializer.Parse(Body, requestType);
		}

		public virtual Stream GetRawBody()
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(Body));
		}
	}
}