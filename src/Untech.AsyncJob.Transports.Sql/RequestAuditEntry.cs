using System;
using Untech.Practices.DataStorage;

namespace Untech.AsyncJob.Transports.Sql
{
	public class RequestAuditEntry : IHasKey
	{
		private RequestAuditEntry() {}

		public RequestAuditEntry(RequestEntry request)
		{
			RequestKey = request.Key;
			Name = request.Name;
			Attributes = request.Attributes;
			Metadata = request.Metadata;
			Body = request.Body;
			Created = request.Created;
		}

		public int Key { get; set; }

		public string RequestKey { get; set; }

		public string Name { get; set; }

		public string Body { get; set; }

		public string Attributes { get; set; }

		public string Metadata { get; set; }

		public DateTimeOffset Created { get; set; }

		public DateTimeOffset Executed { get; set; }

		public bool Succeeded { get; set; }

		public string Error { get; set; }
	}
}