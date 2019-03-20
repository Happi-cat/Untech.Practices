using System;
using System.Collections.Generic;

namespace Untech.AsyncCommmandEngine.Abstractions
{
	public interface IRequestMetadata
	{
		TAttr GetAttribute<TAttr>() where TAttr: Attribute;
		IEnumerable<TAttr> GetAttributes<TAttr>() where TAttr: Attribute;
	}
}