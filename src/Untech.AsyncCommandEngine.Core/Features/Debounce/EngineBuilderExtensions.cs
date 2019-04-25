using System;
using Microsoft.Extensions.Logging;

namespace Untech.AsyncCommandEngine.Features.Debounce
{
	public static class EngineBuilderExtensions
	{
		public static EngineBuilder ThenDebounce(this EngineBuilder builder,
			ILastRunStore lastRunStore)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (lastRunStore == null) throw new ArgumentNullException(nameof(lastRunStore));

			return builder.Then(ctx => new DebounceMiddleware(lastRunStore, ctx.GetLogger()));
		}
	}
}