using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.Practices.CQRS.Dispatching
{
	internal class TypeResolver
	{
		private readonly IServiceProvider _container;

		public TypeResolver(IServiceProvider container)
		{
			_container = container;
		}

		public IEnumerable<T> ResolveMany<T>() where T : class
		{
			try { return ResolveOne<IEnumerable<T>>() ?? Enumerable.Empty<T>(); }
			catch { return Enumerable.Empty<T>(); }
		}

		public T ResolveOne<T>() where T : class
		{
			return (T)_container.GetService(typeof(T));
		}
	}
}