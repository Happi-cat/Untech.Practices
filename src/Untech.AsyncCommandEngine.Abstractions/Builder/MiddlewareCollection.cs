using System;
using System.Collections.Generic;
using System.Linq;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine.Builder
{
	public class MiddlewareCollection
	{
		private readonly List<Func<IBuilderContext, IRequestProcessorMiddleware>> _middlewareCreators = new List<Func<IBuilderContext, IRequestProcessorMiddleware>>();

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="creator">The function that creates <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public MiddlewareCollection Then(Func<IBuilderContext, IRequestProcessorMiddleware> creator)
		{
			_middlewareCreators.Add(creator);
			return this;
		}

		public IEnumerable<IRequestProcessorMiddleware> BuildAll(IBuilderContext context)
		{
			return _middlewareCreators
				.Select(n => n.Invoke(context));
		}
	}
}