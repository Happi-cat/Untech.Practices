using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class CompositeRequestMetadataProvider : IRequestMetadataProvider
	{
		private readonly IReadOnlyCollection<IRequestMetadataProvider> _accessors;

		public CompositeRequestMetadataProvider(IEnumerable<IRequestMetadataProvider> accessors)
		{
			if (accessors == null) throw new ArgumentNullException(nameof(accessors));

			_accessors = accessors.ToList();
		}

		public IRequestMetadata GetMetadata(string requestName)
		{
			if (string.IsNullOrEmpty(requestName)) throw new ArgumentNullException(nameof(requestName));

			var accessors = _accessors.Select(n => n.GetMetadata(requestName));

			return new CompositeRequestMetadata(accessors);
		}
	}
}