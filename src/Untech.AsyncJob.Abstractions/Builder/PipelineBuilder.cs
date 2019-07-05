using System;
using System.Collections.Generic;
using System.Linq;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Builder
{
	using MiddlewareBuilder = Func<IBuilderContext, IRequestProcessorMiddleware>;

	public class PipelineBuilder
	{
		private readonly List<MiddlewareBuilder> _creators = new List<MiddlewareBuilder>();

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="creator">The function that creates <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public PipelineBuilder Then(MiddlewareBuilder creator)
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
