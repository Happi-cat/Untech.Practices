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
			Body = request.Body;
			Created = request.Created;
		}

		public int Key { get; private set; }

		public string RequestKey { get; private set; }

		public string Name { get; private set; }

		public string Body { get; private set; }

		public string Attributes { get; private set; }

		public DateTimeOffset Created { get; private set; }

		public DateTimeOffset Executed { get; set; }

		public bool Succeeded { get; set; }

		public string Error { get; set; }
	}
}