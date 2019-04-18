using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class RequestMetadata : IRequestMetadata
	{
		private readonly IReadOnlyCollection<Attribute> _attributes;

		public RequestMetadata(IEnumerable<Attribute> attributes)
		{
			if (attributes == null) throw new ArgumentNullException(nameof(attributes));

			_attributes = attributes.ToList();
		}

		public TAttr GetAttribute<TAttr>() where TAttr : Attribute
		{
			return GetAttributes<TAttr>().SingleOrDefault();
		}

		public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
		{
			return _attributes.OfType<TAttr>();
		}
	}
}