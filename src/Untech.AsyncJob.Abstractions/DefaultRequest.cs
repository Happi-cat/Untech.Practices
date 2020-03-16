using System;
using System.Collections.Generic;

namespace Untech.AsyncJob
{
	public class DefaultRequest : Request
	{
		public DefaultRequest(
			string name,
			string content = null,
			string contentType = null,
			IReadOnlyDictionary<string, string> attributes = null)
			: this(Guid.NewGuid().ToString(), name, DateTimeOffset.Now, content, contentType, attributes)
		{

		}

		public DefaultRequest(
			string identifier,
			string name,
			DateTimeOffset created,
			string content = null,
			string contentType = null,
			IReadOnlyDictionary<string, string> attributes = null)
		{
			Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Created = created;
			Content = content;
			ContentType = contentType;
			Attributes = attributes;
		}

		public override string Identifier { get; }
		public override string Name { get; }
		public override DateTimeOffset Created { get; }
		public override IReadOnlyDictionary<string, string> Attributes { get; }
		public override string Content { get; }
		public override string ContentType { get; }
	}
}