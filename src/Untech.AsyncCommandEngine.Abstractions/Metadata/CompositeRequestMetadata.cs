using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class CompositeRequestMetadata : IRequestMetadata
	{
		private readonly IReadOnlyCollection<IRequestMetadata> _accessors;

		public CompositeRequestMetadata(IEnumerable<IRequestMetadata> accessors)
		{
			if (accessors == null) throw new ArgumentNullException(nameof(accessors));

			_accessors = accessors.ToList();
		}

		public TAttr GetAttribute<TAttr>() where TAttr : Attribute
		{
			return GetAttributes<TAttr>().SingleOrDefault();
		}

		public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
		{
			return _accessors.SelectMany(n => n.GetAttributes<TAttr>());
		}
	}
}