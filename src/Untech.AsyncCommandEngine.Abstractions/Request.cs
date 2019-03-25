using System;
using System.Collections.Generic;
using System.IO;

namespace Untech.AsyncCommandEngine
{
	public abstract class Request
	{
		public abstract string Identifier { get; }
		public abstract string Name { get; }
		public abstract Stream Body { get; }
		public abstract DateTimeOffset Created { get; }
		public abstract IDictionary<string, string> Attributes { get; }
	}
}