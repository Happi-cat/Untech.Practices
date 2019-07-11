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

		public string Key { get; private set; }

		public string Name { get; private set; }

		public string Body { get; private set; }

		public string Attributes { get; private set; }

		public DateTimeOffset Created { get; private set; }

		public int Priority { get; private set; }

		public DateTimeOffset? ExpiresAfter { get; private set; }

		public DateTimeOffset? ExecuteAfter { get; private set; }
	}
}