using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Untech.AsyncCommandEngine.Transports;

namespace Untech.AsyncCommandEngine
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
		public abstract string Identifier { get; }

		/// <summary>
		/// Gets the request name that is being used during handler selection.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Gets a datetime when the request was created.
		/// </summary>
		public abstract DateTimeOffset Created { get; }

		/// <summary>
		/// Gets a key/value collection that contains request attributes and can be used for sharing attributes.
		/// </summary>
		public abstract IDictionary<string, string> Attributes { get; }

		/// <summary>
		/// Gets key/value collection that can be used to share data between different <see cref="ITransport"/> instances
		/// if composition of transports is being used or between different methods of <see cref="ITransport"/> like Get/Complete.
		/// </summary>
		public IDictionary<object, object> Items { get; }

		/// <summary>
		/// Gets a request payload that was deserialized and converted to <paramref name="requestType"/>.
		/// </summary>
		/// <param name="requestType">The request type to be used for deserialization.</param>
		/// <returns>Deserialized request payload.</returns>
		public abstract object GetBody(Type requestType);

		/// <summary>
		/// Gets a request payload as it is without deserialization.
		/// </summary>
		/// <returns>Request payload stream.</returns>
		public abstract Stream GetRawBody();

		/// <summary>
		/// Gets metadata attributes that were attached directly to that request.
		/// </summary>
		/// <returns>The collection of metadata attributes.</returns>
		public virtual IEnumerable<Attribute> GetAttachedMetadata()
		{
			return Enumerable.Empty<Attribute>();
		}
	}
}