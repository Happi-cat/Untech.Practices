using System.Collections.Generic;

namespace Untech.Practices.CQRS.Dispatching
{
	public interface IHandlersResolver
	{
		T ResolveHandler<T>() where T : class;

		IEnumerable<T> ResolveHandlers<T>() where T : class;
	}
}