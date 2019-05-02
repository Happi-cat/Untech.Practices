using System;
using Microsoft.Extensions.Logging;

namespace Untech.AsyncCommandEngine.Features.WatchDog
{
	internal static class WatchDogMiddlewareLoggerExtensions
	{
		private static readonly Action<ILogger, string, string, TimeSpan, Exception> s_timeoutConfigured;

		static WatchDogMiddlewareLoggerExtensions()
		{
			s_timeoutConfigured = LoggerMessage.Define<string, string, TimeSpan>(LogLevel.Information,
				new EventId(1, "TimeoutConfigured"),
				"Timeout configured for request with trace-id {traceId} and handle {handle} and set to {timeout}");
		}

		public static void TimeoutConfigured(this ILogger logger, Context context, TimeSpan timeout)
		{
			s_timeoutConfigured(logger, context.TraceIdentifier, context.Request.Identifier, timeout, null);
		}
	}
}