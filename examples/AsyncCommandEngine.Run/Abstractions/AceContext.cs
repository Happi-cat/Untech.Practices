using System.Collections.Generic;
using System.Threading;

namespace Untech.AsyncCommmandEngine.Abstractions
{
	public class AceContext
	{
		public AceContext(AceRequest request)
		{
			Request = request;
			Items = new Dictionary<object, object>();
		}

		public AceRequest Request { get; private set; }

		public CancellationToken RequestAborted { get; set; }
		public IDictionary<object, object> Items { get; set; }
	}
}