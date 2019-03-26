using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class CompositeRequestMetadataAccessors : IRequestMetadataAccessors
	{
		private readonly IReadOnlyCollection<IRequestMetadataAccessors> _accessors;

		public CompositeRequestMetadataAccessors(IEnumerable<IRequestMetadataAccessors> accessors)
		{
			if (accessors == null) throw new ArgumentNullException(nameof(accessors));

			_accessors = accessors.ToList();
		}

		public IRequestMetadataAccessor GetMetadata(string requestName)
		{
			var accessors = _accessors.Select(n => n.GetMetadata(requestName));

			return new CompositeRequestMetadataAccessor(accessors);
		}
	}
}