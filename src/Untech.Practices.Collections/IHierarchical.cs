using System.Collections.Generic;

namespace Untech.Practices.Collections
{
	/// <summary>
	///     Represents interface for hierarchical types and provides method that return child elements of same type.
	/// </summary>
	/// <typeparam name="TSelf">Self type.</typeparam>
	public interface IHierarchical<TSelf> where TSelf : IHierarchical<TSelf>
	{
		IEnumerable<TSelf> Elements();
	}
}