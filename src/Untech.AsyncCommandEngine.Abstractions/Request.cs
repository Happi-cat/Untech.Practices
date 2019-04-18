using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Untech.AsyncCommandEngine
{
	public abstract class Request
	{
		public abstract string Identifier { get; }
		public abstract string Name { get; }
		public abstract DateTimeOffset Created { get; }
		public abstract IDictionary<string, string> Attributes { get; }

		public abstract object GetBody(Type requestType);
		public abstract Stream GetRawBody();

		public virtual IEnumerable<Attribute> GetMetadata()
		{
			return Enumerable.Empty<Attribute>();
		}
	}
}