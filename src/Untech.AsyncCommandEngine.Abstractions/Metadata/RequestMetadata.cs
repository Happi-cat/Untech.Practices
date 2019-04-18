using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	/// <summary>
	/// Represents <see cref="IRequestMetadata"/> that's decorates <see cref="IEnumerable{Attribute}"/>.
	/// </summary>
	public class RequestMetadata : IRequestMetadata
	{
		private readonly IReadOnlyCollection<Attribute> _attributes;

		/// <summary>
		/// Initializes a new instance of the <see cref="RequestMetadata"/>
		/// with the specified collection of <see cref="Attribute"/>.
		/// </summary>
		/// <param name="attributes">The collection of attributes to use.</param>
		/// <exception cref="ArgumentNullException"><paramref name="attributes"/> is null.</exception>
		public RequestMetadata(IEnumerable<Attribute> attributes)
		{
			if (attributes == null) throw new ArgumentNullException(nameof(attributes));

			_attributes = attributes.ToList();
		}

		/// <inheritdoc />
		public TAttr GetAttribute<TAttr>() where TAttr : Attribute
		{
			return GetAttributes<TAttr>().SingleOrDefault();
		}

		/// <inheritdoc />
		public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
		{
			return _attributes.OfType<TAttr>();
		}
	}
}