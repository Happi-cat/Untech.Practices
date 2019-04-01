using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Untech.AsyncCommandEngine.Metadata
{
	public interface IRequestMetadataAccessor
	{
		TAttr GetAttribute<TAttr>() where TAttr: Attribute;
		ReadOnlyCollection<TAttr> GetAttributes<TAttr>() where TAttr: Attribute;
	}
}