using System;
using Microsoft.Extensions.Logging;

namespace Untech.AsyncJob.Features.Debounce
{
	internal static class LoggerExtensions
	{
		private static readonly Action<ILogger, string, string, Exception> s_requestDebounced;

		static LoggerExtensions()
		{
			s_requestDebounced = LoggerMessage.Define<string, string>(LogLevel.Information,
				new EventId(1, "RequestDebounced"),
				"Request with trace-id {traceId} and handle {handle} was debounced");
		}

		public static void RequestDebounced(this ILogger logger, Context context)
		{
			s_requestDebounced(logger, context.TraceIdentifier, context.Request.Identifier, null);
		}
	}
}
