using System.Collections.Generic;

namespace Untech.Practices.Collections
{
	/// <summary>
	///     Represents interface for hierarchical types and provides method that return child elements of same type.
	/// </summary>
	/// <typeparam name="T">Self type.</typeparam>
	public interface IHierarchical<T>
		where T : IHierarchical<T>
	{
		IEnumerable<T> GetElements();
	}
}