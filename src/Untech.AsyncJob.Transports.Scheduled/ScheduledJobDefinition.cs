using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.AsyncJob.Formatting.Json;
using Untech.AsyncJob.Metadata.Annotations;

namespace Untech.AsyncJob.Transports.Scheduled
{
	[DataContract]
	public class ScheduledJobDefinition
	{
		public static ScheduledJobDefinition Create<T>(string cron,
			T body = default,
			IDictionary<string, string> attributes = null,
			IEnumerable<MetadataAttribute> metadata = null)
		{
			var definition = new ScheduledJobDefinition
			{
				Cron = cron,
				Name = typeof(T).FullName,
				Attributes = attributes,
				Metadata = metadata
			};

			definition.SetBody(body);

			return definition;
		}

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public IDictionary<string, string> Attributes { get; set; }

		[DataMember]
		public IEnumerable<MetadataAttribute> Metadata { get; set; }

		[DataMember]
		public string Cron { get; set; }

		[DataMember]
		public string Content { get; set; }
		[DataMember]
		public string ContentType { get; set; }

		public virtual void SetBody(object value)
		{
			Content = JsonRequestContentFormatter.Default.Serialize(value);
			ContentType = JsonRequestContentFormatter.Default.Type;
		}
	}
}
