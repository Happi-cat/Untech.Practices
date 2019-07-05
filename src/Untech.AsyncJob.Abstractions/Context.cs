using System;
using System.Collections.Generic;
using System.Threading;
using Untech.AsyncJob.Metadata;

namespace Untech.AsyncJob
{
	/// <summary>
	/// Represents context for the current request that should be processed.
	/// </summary>
	public class Context
	{
		private IDictionary<object, object> _items;

		public Context(Request request)
			: this(request, NullRequestMetadataProvider.Instance)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/>
		/// with the specified <paramref name="request"/> and <paramref name="metadataProvider"/>.
		/// </summary>
		/// <param name="request">The request to handle.</param>
		/// <param name="metadataProvider">The request metadata provider.</param>
		public Context(Request request, IRequestMetadataProvider metadataProvider)
		{
			if (request == null)throw new ArgumentNullException(nameof(request));
			if (request.Name == null) throw new ArgumentNullException(nameof(request.Name));
			if (metadataProvider == null) throw new ArgumentNullException(nameof(metadataProvider));

			TraceIdentifier = Guid.NewGuid().ToString();

			Request = request;
			RequestName = request.Name;
			RequestMetadata = new CompositeRequestMetadata(new[]
			{
				new RequestMetadata(request.GetAttachedMetadata()),
				metadataProvider.GetMetadata(request.Name)
			});

			_items = new Dictionary<object, object>();
		}

		/// <summary>
		/// Gets the current <see cref="Request"/> that should be processed.
		/// </summary>
		public Request Request { get; private set; }

		/// <summary>
		/// Gets the name of the current <see cref="Request"/>.
		/// </summary>
		public string RequestName { get; private set; }

		/// <summary>
		/// Gets the metadata that is associated with the current <see cref="Request"/>.
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
