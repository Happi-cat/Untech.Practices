using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncCommandEngine;
using Untech.AsyncCommandEngine.Processing;

namespace AsyncCommandEngine.Run
{
	internal class DemoMiddleware : IRequestProcessorMiddleware
	{
		private static int s_nextTraceId = 1;

		private readonly ILogger _logger;

		public DemoMiddleware(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<DemoMiddleware>();
		}

		public async Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			context.TraceIdentifier = Interlocked.Increment(ref s_nextTraceId).ToString();

			_logger.Log(LogLevel.Debug, "{0} starting: {1}", context.TraceIdentifier, GetBodyAsJson(context.Request));
			try
			{
				await next(context);
			}
			catch (OperationCanceledException e)
			{
				_logger.Log(LogLevel.Warning, e, "{0} was canceled", context.TraceIdentifier, e.Message);
			}
			catch (Exception e)
			{
				_logger.Log(LogLevel.Error, e, "{0} crashed: {1}", context.TraceIdentifier, e.Message);
			}
		}

		private static string GetBodyAsJson(Request request)
		{
			using (var stream = request.GetRawBody())
			using (var reader = new StreamReader(stream))
				return reader.ReadToEnd();
		}
	}
}