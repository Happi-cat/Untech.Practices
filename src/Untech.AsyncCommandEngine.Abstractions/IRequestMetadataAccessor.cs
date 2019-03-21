using System;
using System.Collections.Generic;

namespace Untech.AsyncCommandEngine
{
	public interface IRequestMetadata<TRequest>
	{

	}

	public interface IRequestMetadataAccessor
	{
		TAttr GetAttribute<TAttr>() where TAttr: Attribute;
		IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr: Attribute;
	}
}