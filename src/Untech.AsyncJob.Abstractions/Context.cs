using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Untech.AsyncJob.Metadata;

namespace Untech.AsyncJob
{
	/// <summary>
	/// Represents context for the current request that should be processed.
	/// </summary>
	public class Context
	{
		private string _traceIdentifier;
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
		public Context([NotNull] Request request, [NotNull] IRequestMetadataProvider metadataProvider)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));
			if (metadataProvider == null) throw new ArgumentNullException(nameof(metadataProvider));

			Request = request;
			RequestMetadata = new CompositeRequestMetadata(new[]
			{
				new RequestMetadata(request.GetAttachedMetadata()),
				metadataProvider.GetMetadata(request.Name)
			});
		}

		/// <summary>
		/// Gets the current <see cref="Request"/> that should be processed.
		/// </summary>
		[NotNull]
		public Request Request { get; }

		/// <summary>
		/// Gets the name of the current <see cref="Request"/>.
		/// </summary>
		[NotNull]
		public string RequestName => Request.Name;

		/// <summary>
		/// Gets the metadata that is associated with the current <see cref="Request"/>.
		/// </summary>
		[NotNull]
		public IRequestMetadata RequestMetadata { get; }

		/// <summary>
		/// Gets or sets <see cref="CancellationToken"/> that can be used for request cancellation.
		/// </summary>
		public virtual CancellationToken Aborted { get; set; }

		/// <summary>
		/// Gets or sets trace identifier for current context.
		/// </summary>
		public virtual string TraceIdentifier
		{
			get => _traceIdentifier ?? Request.Identifier;
			set => _traceIdentifier = value;
		}

		/// <summary>
		/// Gets or sets key/value collection that can be used to share data within the scope where context is available.
		/// </summary>
		public virtual IDictionary<object, object> Items
		{
			get => _items = _items ?? new Dictionary<object, object>();
			set => _items = value;
		}
	}
}
