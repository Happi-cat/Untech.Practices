using System;
using System.Collections.Generic;
using System.Threading;

namespace Untech.AsyncCommandEngine
{
	public class AceContext
	{
		private IDictionary<object, object> _items;

		public AceContext(AceRequest request)
		{
			Request = request;

			_items = new Dictionary<object, object>();
		}

		public AceRequest Request { get; private set; }

		public CancellationToken RequestAborted { get; set; }

		public IDictionary<object, object> Items
		{
			get => _items;
			set => _items = value ?? throw new ArgumentNullException(nameof(value));
		}
	}
}