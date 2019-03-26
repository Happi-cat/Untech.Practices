using System;
using Microsoft.Extensions.Logging;

namespace Untech.AsyncCommandEngine.Features.Debounce
{
	public static class EngineBuilderExtensions
	{
		public static EngineBuilder UseDebounce(this EngineBuilder builder,
			ILastRunStore lastRunStore,
			ILoggerFactory loggerFactory)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (lastRunStore == null) throw new ArgumentNullException(nameof(lastRunStore));
			if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

			return builder.Use(() => new DebounceMiddleware(lastRunStore, loggerFactory));
		}
	}
}