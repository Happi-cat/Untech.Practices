using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Untech.AsyncCommandEngine.Metadata
{
	public interface IRequestMetadata
	{
		TAttr GetAttribute<TAttr>() where TAttr: Attribute;
		IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr: Attribute;
	}
}