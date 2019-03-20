using System;
using AsyncCommandEngine.Run;

namespace AsyncCommandEngine.Examples.Features.Debounce
{
	public static class AceBuilderExtensions
	{
		public static AceBuilder UseDebounce(this AceBuilder builder, ILastRunStore lastRunStore)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (lastRunStore == null) throw new ArgumentNullException(nameof(lastRunStore));

			return builder.Use(() => new DebounceMiddleware(lastRunStore));
		}
	}
}