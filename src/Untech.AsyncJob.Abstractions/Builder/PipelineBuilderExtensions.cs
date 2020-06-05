using System;
using System.Threading.Tasks;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Builder
{
	public static class PipelineBuilderExtensions
	{
		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="builder">The builder to use for middleware registration.</param>
		/// <param name="middleware">The instance of the <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public static PipelineBuilder Add(this PipelineBuilder builder,
			IRequestProcessorMiddleware middleware)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (middleware == null) throw new ArgumentNullException(nameof(middleware));

			return builder.Add(ctx => middleware);
		}

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="builder">The builder to use for middleware registration.</param>
		/// <param name="middleware">The <see cref="IRequestProcessor"/> middleware.</param>
		/// <returns></returns>
		public static PipelineBuilder Add(this PipelineBuilder builder,
			Func<Context, RequestProcessorCallback, Task> middleware)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (middleware == null) throw new ArgumentNullException(nameof(middleware));

			return builder.Add(ctx => new AdHocRequestProcessorMiddleware(middleware));
		}

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="builder">The builder to use for middleware registration.</param>
		/// <param name="creator">The function that creates <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public static PipelineBuilder Add(this PipelineBuilder builder,
			Func<IServiceProvider, Func<Context, RequestProcessorCallback, Task>> creator)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (creator == null) throw new ArgumentNullException(nameof(creator));

			return builder.Add(ctx => new AdHocRequestProcessorMiddleware(creator(ctx)));
		}
	}
}
