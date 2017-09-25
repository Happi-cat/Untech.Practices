using System.Collections.Generic;

namespace Untech.Practices.Collections
{
	public interface IHierarchical<T> : IEnumerable<T>
		where T : IHierarchical<T>
	{
	}
}