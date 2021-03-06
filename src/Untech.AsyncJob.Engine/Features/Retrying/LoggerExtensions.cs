﻿using System;
using Microsoft.Extensions.Logging;

namespace Untech.AsyncJob.Features.Retrying
{
	internal static class LoggerExtensions
	{
		private static readonly Action<ILogger, string, string, int, Exception> s_willRetry;

		static LoggerExtensions()
		{
			s_willRetry = LoggerMessage.Define<string, string, int>(LogLevel.Warning,
				new EventId(1, "WillRetry"),
				"Going to retry request with {traceId} and handle {handle} due to error (attempt {attempt})");
		}

		public static void WillRetry(this ILogger logger, Context context, int attempt, Exception exception)
		{
			s_willRetry(logger, context.TraceIdentifier, context.Request.Identifier, attempt, exception);
		}
	}
}
