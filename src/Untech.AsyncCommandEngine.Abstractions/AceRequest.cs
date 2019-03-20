using System;

namespace Untech.AsyncCommandEngine
{
	public class AceRequest
	{
		public AceRequest(string typeName, DateTimeOffset created, IRequestMetadata metadata)
		{
			TypeName = typeName;
			Created = created;
			Metadata = metadata;
		}

		public string TypeName { get; private set; }
		public IRequestMetadata Metadata { get; private set; }
		public DateTimeOffset Created { get; private set; }
		public string Body { get; private set; }
	}
}