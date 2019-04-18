using System;
using System.Collections.Generic;
using System.Threading;
using Untech.AsyncCommandEngine.Metadata;

namespace Untech.AsyncCommandEngine
{
	/// <summary>
	/// Represents context of the current request to be processed.
	/// </summary>
	public class Context
	{
		private IDictionary<object, object> _items;

		public Context(Request request, IRequestMetadata requestMetadata)
		{
			TraceIdentifier = Guid.NewGuid().ToString();

			Request = request ?? throw new ArgumentNullException(nameof(request));
			RequestName = request.Name ?? throw new ArgumentNullException(nameof(request.Name));
			RequestMetadata = requestMetadata ?? throw new ArgumentNullException(nameof(requestMetadata));

			_items = new Dictionary<object, object>();
		}

		/// <summary>
		/// Gets the current <see cref="Request"/>.
		/// </summary>
		public Request Request { get; private set; }

		/// <summary>
		/// Gets the name of the current <see cref="Request"/>.
		/// </summary>
		public string RequestName { get; private set; }

		/// <summary>
		/// Gets the metadata of the current <see cref="Request"/>.
		/// </summary>
		public IRequestMetadata RequestMetadata { get; private set; }

		/// <summary>
		/// Gets or sets <see cref="CancellationToken"/> that can be used for request cancellation.
		/// </summary>
		public CancellationToken Aborted { get; set; }

		/// <summary>
		/// Gets or sets trace identifier for current context.
		/// </summary>
		public string TraceIdentifier { get; set; }

		/// <summary>
		/// Gets or sets key/value collection that can be used to share data within the scope where context is available.
		/// </summary>
		public IDictionary<object, object> Items
		{
			get => _items;
			set => _items = value ?? throw new ArgumentNullException(nameof(value));
		}
	}
}