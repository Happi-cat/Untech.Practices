using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Untech.AsyncJob.Metadata.Annotations;

namespace Untech.AsyncJob.Metadata
{
	/// <summary>
	/// Represents <see cref="IRequestMetadata"/> that's decorates <see cref="IEnumerable{Attribute}"/>.
	/// </summary>
	public class RequestMetadata : IRequestMetadata, IEnumerable<Attribute>
	{
		private readonly ICollection<Attribute> _attributes;

		public RequestMetadata()
		{
			_attributes = new List<Attribute>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RequestMetadata"/>
		/// with the specified collection of <see cref="Attribute"/>.
		/// </summary>
		/// <param name="attributes">The collection of attributes to use.</param>
		/// <exception cref="ArgumentNullException"><paramref name="attributes"/> is null.</exception>
		public RequestMetadata(IEnumerable<Attribute> attributes)
		{
			if (attributes == null)
				throw new ArgumentNullException(nameof(attributes));

			_attributes = attributes.ToList();
		}

		/// <inheritdoc />
		public TAttr GetAttribute<TAttr>() where TAttr : MetadataAttribute
		{
			return GetAttributes<TAttr>().SingleOrDefault();
		}

		/// <inheritdoc />
		public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : MetadataAttribute
		{
			return _attributes.OfType<TAttr>();
		}

		public RequestMetadata Add(Attribute attribute)
		{
			if (attribute == null)
				throw new ArgumentNullException(nameof(attribute));

			_attributes.Add(attribute);

			return this;
		}

		public IEnumerator<Attribute> GetEnumerator()
		{
			return _attributes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
