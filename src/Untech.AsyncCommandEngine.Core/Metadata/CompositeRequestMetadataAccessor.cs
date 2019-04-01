using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class CompositeRequestMetadataAccessor : IRequestMetadataAccessor
	{
		private readonly IReadOnlyCollection<IRequestMetadataAccessor> _accessors;

		public CompositeRequestMetadataAccessor(IEnumerable<IRequestMetadataAccessor> accessors)
		{
			if (accessors == null) throw new ArgumentNullException(nameof(accessors));

			_accessors = accessors.ToList();
		}

		public TAttr GetAttribute<TAttr>() where TAttr : Attribute
		{
			return _accessors
				.Select(n => n.GetAttribute<TAttr>())
				.SingleOrDefault(n => !ReferenceEquals(n, null));
		}

		public ReadOnlyCollection<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
		{
			return _accessors
				.SelectMany(n => n.GetAttributes<TAttr>())
				.ToList()
				.AsReadOnly();
		}
	}
}