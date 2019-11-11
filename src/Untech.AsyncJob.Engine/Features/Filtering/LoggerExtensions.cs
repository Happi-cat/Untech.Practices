using System;
using Microsoft.Extensions.Logging;

namespace Untech.AsyncJob.Features.Filtering
{
	internal static class LoggerExtensions
	{
		private static readonly Action<ILogger, string, string, Exception> s_requestFiltered;

		static LoggerExtensions()
		{
			s_requestFiltered = LoggerMessage.Define<string, string>(LogLevel.Information,
				new EventId(1, "RequestFiltered"),
				"Request with trace-id {traceId} and handle {handle} was filtered");
		}

		public static void RequestFiltered(this ILogger logger, Context context)
		{
			s_requestFiltered(logger, context.TraceIdentifier, context.Request.Identifier, null);
		}
	}
}
