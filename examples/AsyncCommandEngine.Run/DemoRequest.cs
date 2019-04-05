using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Untech.AsyncCommandEngine;

namespace AsyncCommandEngine.Run
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

		public override object GetBody(Type requestType)
		{
			return _body;
		}

		public override Stream GetRawBody()
		{
			var body = JsonConvert.SerializeObject(_body);
			return new MemoryStream(Encoding.UTF8.GetBytes(body));
		}
	}
}