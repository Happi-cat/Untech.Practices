using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class CompositeRequestMetadataAccessors : IRequestMetadataAccessors
	{
		private readonly IEnumerable<IRequestMetadataAccessors> _accessors;

		public CompositeRequestMetadataAccessors(IEnumerable<IRequestMetadataAccessors> accessors)
		{
			_accessors = accessors.ToList();
		}

		public IRequestMetadataAccessor GetMetadata(string requestName)
		{
			var accessors = _accessors.Select(n => n.GetMetadata(requestName));

			return new CompositeRequestMetadataAccessor(accessors);
		}

		private class CompositeRequestMetadataAccessor : IRequestMetadataAccessor
		{
			private readonly IEnumerable<IRequestMetadataAccessor> _accessors;

			public CompositeRequestMetadataAccessor(IEnumerable<IRequestMetadataAccessor> accessors)
			{
				_accessors = accessors.ToList();
			}

			public TAttr GetAttribute<TAttr>() where TAttr : Attribute
			{
				return _accessors
					.Select(n => n.GetAttribute<TAttr>())
					.FirstOrDefault(n => !ReferenceEquals(n, null));
			}

			public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
			{
				return _accessors.SelectMany(n => n.GetAttributes<TAttr>());
			}
		}
	}
}