using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

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
		public IEnumerable<Attribute> Metadata { get; set; }

		[DataMember]
		public TimeSpan Interval { get; set; }

		[DataMember]
		public string Body { get; set; }

		public object GetBody(Type requestType)
		{
			return JsonConvert.DeserializeObject(Body, requestType);
		}

		public Stream GetRawBody()
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(Body));
		}
	}
}