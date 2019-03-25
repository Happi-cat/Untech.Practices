using System;
using System.Collections.Generic;

namespace Untech.AsyncCommandEngine.Metadata
{
	public interface IRequestMetadataAccessor
	{
		TAttr GetAttribute<TAttr>() where TAttr: Attribute;
		IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr: Attribute;
	}
}