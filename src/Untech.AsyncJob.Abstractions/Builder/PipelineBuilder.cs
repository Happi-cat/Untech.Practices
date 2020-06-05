using System;
using System.Collections;
using System.Collections.Generic;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Builder
{
	using MiddlewareBuilder = Func<IServiceProvider, IRequestProcessorMiddleware>;

	public class PipelineBuilder : IEnumerable<MiddlewareBuilder>
	{
		private readonly List<MiddlewareBuilder> _creators = new List<MiddlewareBuilder>();

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="creator">The function that creates <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public PipelineBuilder Add(MiddlewareBuilder creator)
		{
			if (creator == null) throw new ArgumentNullException(nameof(creator));

			_creators.Add(creator);
			return this;
		}

		public IEnumerator<MiddlewareBuilder> GetEnumerator()
		{
			return _creators.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
