using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

namespace Untech.AsyncJob.Transports.Sql
{
	internal class SqlRequest : Request
	{
		private readonly string _body;

		public SqlRequest(RequestEntry entry)
		{
			Identifier = entry.Key;
			Name = entry.Name;
			Created = entry.Created;

			Attributes = JsonSerializer.Parse<Dictionary<string, string>>(entry.Attributes);
			_body = entry.Body;

			Items[typeof(RequestEntry)] = entry;
		}

		public override string Identifier { get; }
		public override string Name { get; }
		public override DateTimeOffset Created { get; }

		public override IDictionary<string, string> Attributes { get; }

		public override object GetBody(Type requestType)
		{
			return JsonSerializer.Parse(_body, requestType);
		}

		public override Stream GetRawBody()
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(_body));
		}
	}
}