using System;
using System.Linq;
using System.Text.Json.Serialization;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.DataStorage;

namespace Untech.AsyncJob.Transports.Sql
{
	public class RequestEntry : IHasKey<string>
	{
		private RequestEntry() {}

		public RequestEntry(object payload, QueueOptions options = null)
		{
			Key = Guid.NewGuid().ToString("B");
			Name = payload.GetType().FullName;
			Created = DateTimeOffset.Now;

			Attributes = JsonSerializer.ToString(options?.Advanced?.ToDictionary(n => n.Key, n => Convert.ToString((object)n.Value)));
			Priority = options?.Priority ?? 0;
			ExecuteAfter = options?.ExecuteAfter != null ? Created + options.ExecuteAfter : null;
			ExpiresAfter = options?.ExpiresAfter != null ? Created + options.ExpiresAfter : null;

			Body = JsonSerializer.ToString(payload);
		}

		public string Key { get; set; }

		public string Name { get; set; }

		public string Body { get; set; }

		public string Attributes { get; set; }

		public string Metadata { get; set; }

		public DateTimeOffset Created { get; set; }

		public int Priority { get; set; }

		public DateTimeOffset? ExpiresAfter { get; set; }

		public DateTimeOffset? ExecuteAfter { get; set; }

		public bool Completed { get; set; }

		public string Error { get; set; }
	}
}