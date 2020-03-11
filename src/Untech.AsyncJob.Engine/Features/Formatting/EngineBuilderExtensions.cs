using System;
using System.Collections.Generic;
using System.Linq;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Formatting;

namespace Untech.AsyncJob.Features.Formatting
{
	public static class EngineBuilderExtensions
	{
		public static PipelineBuilder ThenResolveFormatter(this PipelineBuilder builder,
			IRequestContentFormatter formatter)
		{
			return builder.ThenResolveFormatter(Enumerable.Empty<IRequestContentFormatter>(), formatter);
		}

		public static PipelineBuilder ThenResolveFormatter(this PipelineBuilder builder,
			IEnumerable<IRequestContentFormatter> formatters, IRequestContentFormatter fallbackFormatter)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (formatters == null) throw new ArgumentNullException(nameof(formatters));
			if (fallbackFormatter == null) throw new ArgumentNullException(nameof(fallbackFormatter));

			return builder.Then(ctx => new FormattingMiddleware(formatters, fallbackFormatter));
		}
	}
}