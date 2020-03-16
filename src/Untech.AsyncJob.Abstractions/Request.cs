using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Untech.AsyncJob.Metadata.Annotations;
using Untech.AsyncJob.Transports;

namespace Untech.AsyncJob
{
	/// <summary>
	/// Represents base class for requests that should be processed asynchronously.
	/// </summary>
	public abstract class Request
	{
		protected Request()
		{
			Items = new Dictionary<object, object>();
		}

		/// <summary>
		/// Gets the request identifier that can be used for request identification and tracking.
		/// </summary>
		[NotNull]
		public abstract string Identifier { get; }

		/// <summary>
		/// Gets the request name that is being used during handler selection.
		/// </summary>
		[NotNull]
		public abstract string Name { get; }

		/// <summary>
		/// Gets a datetime when the request was created.
		/// </summary>
		public abstract DateTimeOffset Created { get; }

		/// <summary>
		/// Gets a key/value collection that contains request attributes and can be used for sharing attributes.
		/// </summary>
		[CanBeNull]
		public abstract IReadOnlyDictionary<string, string> Attributes { get; }

		/// <summary>
		/// Gets serialized content.
		/// </summary>
		[CanBeNull]
		public abstract string Content { get; }

		/// <summary>
		/// Gets serializer type that was used for content serialization.
		/// </summary>
		[CanBeNull]
		public abstract string ContentType { get; }

		/// <summary>
		/// Gets key/value collection that can be used to share data between different <see cref="ITransport"/> instances
		/// if composition of transports is being used or between different methods of <see cref="ITransport"/> like Get/Complete.
		/// </summary>
		[NotNull]
		public IDictionary<object, object> Items { get; }

		/// <summary>
		/// Gets metadata attributes that were attached directly to that request.
		/// </summary>
		/// <returns>The collection of metadata attributes.</returns>
		[NotNull]
		public virtual IEnumerable<MetadataAttribute> GetAttachedMetadata()
		{
			return Enumerable.Empty<MetadataAttribute>();
		}
	}
}
