using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	public class NullRequestMetadataAccessor : IRequestMetadataAccessor
	{
		public static readonly IRequestMetadataAccessor Default = new NullRequestMetadataAccessor();

		public TAttr GetAttribute<TAttr>() where TAttr : Attribute
		{
			return default;
		}

		public IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr : Attribute
		{
			return Enumerable.Empty<TAttr>();
		}
	}
}