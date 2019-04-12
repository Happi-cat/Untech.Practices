using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Untech.AsyncCommandEngine.Metadata
{
	public sealed class NullRequestMetadata : IRequestMetadata
	{
		public static readonly IRequestMetadata Instance = new NullRequestMetadata();

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