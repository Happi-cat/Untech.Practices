using System;
using System.Collections.Generic;

namespace Untech.Practices.CQRS.Dispatching.RequestExecutors
{
	internal class HandlersResolverWrapper : IHandlersResolver
	{
		private readonly IHandlersResolver _inner;

		public HandlersResolverWrapper(IHandlersResolver inner)
		{
			_inner = inner;
		}

		public T ResolveHandler<T>() where T : class
		{
			var result = _inner.ResolveHandler<T>();
			if (result == null) throw new InvalidOperationException($"Unable to resolve {typeof(T)}");
			return result;
		}

		public IEnumerable<T> ResolveHandlers<T>() where T : class
		{
			var result = _inner.ResolveHandlers<T>();
			if (result == null) throw new InvalidOperationException($"Unable to resolve {typeof(T)}");
			return result;
		}
	}
}