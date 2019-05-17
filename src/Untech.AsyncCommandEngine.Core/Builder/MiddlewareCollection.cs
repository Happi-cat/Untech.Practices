using System;
using System.Collections.Generic;
using System.Linq;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine.Builder
{
	public class MiddlewareCollection
	{
		private readonly List<Func<IBuilderContext, IRequestProcessorMiddleware>> _middlewareCreators = new List<Func<IBuilderContext, IRequestProcessorMiddleware>>();
		private Func<IBuilderContext, IRequestProcessorMiddleware> _finalMiddlewareCreator;

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

		public void Final(ICqrsStrategy strategy)
		{
			_finalMiddlewareCreator = ctx => new CqrsMiddleware(strategy);
		}

		public void Final(Func<IBuilderContext, ICqrsStrategy> strategy)
		{
			_finalMiddlewareCreator = ctx => new CqrsMiddleware(strategy(ctx));
		}

		internal IEnumerable<IRequestProcessorMiddleware> BuildSteps(IBuilderContext context)
		{
			if (_finalMiddlewareCreator == null) throw NoFinalStepError();

			return _middlewareCreators
				.Concat(new[] { _finalMiddlewareCreator })
				.Select(n => n.Invoke(context));
		}

		private static InvalidOperationException NoFinalStepError()
		{
			return new InvalidOperationException("Final middleware wasn't configured");
		}
	}
}