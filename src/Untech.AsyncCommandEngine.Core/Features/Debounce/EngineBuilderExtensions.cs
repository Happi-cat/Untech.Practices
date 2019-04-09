using System;
using Microsoft.Extensions.Logging;

namespace Untech.AsyncCommandEngine.Features.Debounce
{
	public static class EngineBuilderExtensions
	{
		public static EngineBuilder UseDebounce(this EngineBuilder builder,
			ILastRunStore lastRunStore)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (lastRunStore == null) throw new ArgumentNullException(nameof(lastRunStore));

			return builder.Use(ctx => new DebounceMiddleware(lastRunStore, ctx.GetLogger()));
		}
	}
}