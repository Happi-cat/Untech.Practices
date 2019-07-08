using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.AsyncJob.Transports.InProcess
{
	internal class InProcessRequest:  Request
	{
		private readonly string _payload;

		public InProcessRequest(object payload, QueueOptions options)
		{
			Identifier = Guid.NewGuid().ToString("B");
			Name = payload.GetType().FullName;
			Created = DateTimeOffset.Now;
			_payload = JsonSerializer.ToString(payload);

			QueueOptions = options;
			Attributes = options?.Advanced?.ToDictionary(n => n.Key, n => Convert.ToString(n.Value));
		}

		public override string Identifier { get; }
		public override string Name { get; }
		public override DateTimeOffset Created { get; }

		public override IDictionary<string, string> Attributes { get; }

		public QueueOptions QueueOptions { get; }

		public int GetPriority()
		{
			return QueueOptions?.Priority ?? 0;
		}

		public bool IsExpired()
		{
			return QueueOptions?.ExpiresAfter != null
				&& Created + QueueOptions.ExpiresAfter <= DateTimeOffset.Now;
		}

		public bool IsExecuteAfterReached()
		{
			return QueueOptions?.ExecuteAfter == null
				|| Created + QueueOptions.ExecuteAfter <= DateTimeOffset.Now;
		}

		public override object GetBody(Type requestType)
		{
			return JsonSerializer.Parse(_payload, requestType);
		}

		public override Stream GetRawBody()
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(_payload));
		}
	}
}