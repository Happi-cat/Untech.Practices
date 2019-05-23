using System;
using System.Collections.Generic;
using System.Linq;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine.Builder
{
	public class PipelineBuilder
	{
		private readonly List<Func<IBuilderContext, IRequestProcessorMiddleware>> _creators = new List<Func<IBuilderContext, IRequestProcessorMiddleware>>();

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="creator">The function that creates <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public PipelineBuilder Then(Func<IBuilderContext, IRequestProcessorMiddleware> creator)
		{
			if (creator == null) throw new ArgumentNullException(nameof(creator));

			_creators.Add(creator);
			return this;
		}

		public IEnumerable<IRequestProcessorMiddleware> BuildAll(IBuilderContext context)
		{
			return _creators.Select(n => n.Invoke(context));
		}
	}
}