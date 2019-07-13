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
		private readonly object _payload;
		private string _serializedPayload;

		private readonly int _priority;
		private readonly TimeSpan? _expiresAfter;
		private readonly TimeSpan? _executeAfter;

		public InProcessRequest(object payload, QueueOptions options)
		{
			if (payload == null) throw new ArgumentNullException(nameof(payload));

			Identifier = Guid.NewGuid().ToString("B");
			Name = payload.GetType().FullName;
			Created = DateTimeOffset.Now;
			_payload = payload;

			Attributes = options?.Advanced?.ToDictionary(n => n.Key, n => Convert.ToString(n.Value));
			_priority = options?.Priority ?? 0;
			_expiresAfter = options?.ExpiresAfter;
			_executeAfter = options?.ExecuteAfter;
		}

		public override string Identifier { get; }
		public override string Name { get; }
		public override DateTimeOffset Created { get; }

		public override IDictionary<string, string> Attributes { get; }

		public int GetPriority()
		{
			return _priority;
		}

		public bool IsExpired()
		{
			return _expiresAfter != null && Created + _expiresAfter <= DateTimeOffset.Now;
		}

		public bool IsExecuteAfterReached()
		{
			return _executeAfter == null || Created + _executeAfter <= DateTimeOffset.Now;
		}

		public override object GetBody(Type requestType)
		{
			if (requestType.IsInstanceOfType(_payload)) return _payload;

			throw new ArgumentException($"Payload type is {_payload.GetType()} when request is {requestType}");
		}

		public override Stream GetRawBody()
		{
			if (_serializedPayload == null) _serializedPayload = JsonSerializer.ToString(_payload);

			return new MemoryStream(Encoding.UTF8.GetBytes(_serializedPayload));
		}
	}
}