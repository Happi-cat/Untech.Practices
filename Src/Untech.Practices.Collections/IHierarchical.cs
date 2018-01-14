using System.Collections.Generic;

namespace Untech.Practices.Collections
{
	public interface IHierarchical<T>
		where T : IHierarchical<T>
	{
		IEnumerable<T> GetElements();
	}
}