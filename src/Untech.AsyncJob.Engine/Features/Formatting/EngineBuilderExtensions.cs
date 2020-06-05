using System;
using System.Collections.Generic;
using System.Linq;
using Untech.AsyncJob.Builder;
using Untech.AsyncJob.Formatting;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.Formatting
{
	public static class EngineBuilderExtensions
	{
		public static IRegistrar<IRequestProcessorMiddleware> AddResolveFormatter(this IRegistrar<IRequestProcessorMiddleware> builder,
			IRequestContentFormatter formatter)
		{
			return builder.AddResolveFormatter(Enumerable.Empty<IRequestContentFormatter>(), formatter);
		}

		public static IRegistrar<IRequestProcessorMiddleware> AddResolveFormatter(this IRegistrar<IRequestProcessorMiddleware> builder,
			IEnumerable<IRequestContentFormatter> formatters, IRequestContentFormatter fallbackFormatter)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (formatters == null) throw new ArgumentNullException(nameof(formatters));
			if (fallbackFormatter == null) throw new ArgumentNullException(nameof(fallbackFormatter));

			return builder.Add(ctx => new FormattingMiddleware(formatters, fallbackFormatter));
		}
	}
}