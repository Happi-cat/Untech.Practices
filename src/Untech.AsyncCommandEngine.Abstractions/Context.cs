using System;
using System.Collections.Generic;
using System.Threading;
using Untech.AsyncCommandEngine.Metadata;

namespace Untech.AsyncCommandEngine
{
	public class Context
	{
		private IDictionary<object, object> _items;

		public Context(Request request, IRequestMetadata requestMetadata)
		{
			Request = request;
			RequestName = request.Name;
			RequestMetadata = requestMetadata;

			_items = new Dictionary<object, object>();
		}

		public Request Request { get; private set; }

		public string RequestName { get; private set; }
		public IRequestMetadata RequestMetadata { get; private set; }

		public CancellationToken Aborted { get; set; }
		public string TraceIdentifier { get; set; }

		public IDictionary<object, object> Items
		{
			get => _items;
			set => _items = value ?? throw new ArgumentNullException(nameof(value));
		}
	}
}