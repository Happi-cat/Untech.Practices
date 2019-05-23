using System;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine.Builder
{
	public static class MiddlewareCollectionExtensions {

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="builder">The builder to use for middleware registration.</param>
		/// <param name="middleware">The instance of the <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public static MiddlewareCollection Then(this MiddlewareCollection builder,
			IRequestProcessorMiddleware middleware)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (middleware == null) throw new ArgumentNullException(nameof(middleware));

			return builder.Then(ctx => middleware);
		}

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="builder">The builder to use for middleware registration.</param>
		/// <param name="middleware">The <see cref="IRequestProcessor"/> middleware.</param>
		/// <returns></returns>
		public static MiddlewareCollection Then(this MiddlewareCollection builder,
			Func<Context, RequestProcessorCallback, Task> middleware)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (middleware == null) throw new ArgumentNullException(nameof(middleware));

			return builder.Then(ctx => new AdHocRequestProcessorMiddleware(middleware));
		}

		/// <summary>
		/// Registers middleware.
		/// </summary>
		/// <param name="builder">The builder to use for middleware registration.</param>
		/// <param name="creator">The function that creates <see cref="IRequestProcessorMiddleware"/>.</param>
		/// <returns></returns>
		public static MiddlewareCollection Then(this MiddlewareCollection builder,
			Func<IBuilderContext, Func<Context, RequestProcessorCallback, Task>> creator)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (creator == null) throw new ArgumentNullException(nameof(creator));

			return builder.Then(ctx => new AdHocRequestProcessorMiddleware(creator(ctx)));
		}
	}
}