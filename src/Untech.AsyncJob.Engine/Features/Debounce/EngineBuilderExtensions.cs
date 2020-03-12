using System;
using Untech.AsyncJob.Builder;

namespace Untech.AsyncJob.Features.Debounce
{
	/// <summary>
	/// Extensions for debounce feature configuration.
	/// </summary>
	public static class EngineBuilderExtensions
	{
		/// <summary>
		/// Register Debounce middleware with the specified <paramref name="lastRunStore"/>.
		/// </summary>
		/// <param name="builder">The builder to use for registration.</param>
		/// <param name="lastRunStore">The last run store to use.</param>
		/// <returns><paramref name="builder"/></returns>
		/// <exception cref="ArgumentNullException">
		/// 	<paramref name="builder"/> or <paramref name="lastRunStore"/> is null.
		/// </exception>
		public static PipelineBuilder ThenDebounce(this PipelineBuilder builder,
			ILastRunStore lastRunStore)
		{
			if (builder == null)
				throw new ArgumentNullException(nameof(builder));
			if (lastRunStore == null)
				throw new ArgumentNullException(nameof(lastRunStore));

			return builder.Then(ctx => new DebounceMiddleware(lastRunStore, ctx.GetLogger()));
		}
	}
}
