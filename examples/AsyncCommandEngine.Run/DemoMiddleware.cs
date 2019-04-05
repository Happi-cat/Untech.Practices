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

			var body = GetBodyAsJson(context.Request);
			var request = $"{context.TraceIdentifier}|{context.RequestName}|{body}";

			_logger.Log(LogLevel.Debug, "{0} starting", request);
			try
			{
				await next(context);
			}
			catch (OperationCanceledException e)
			{
				_logger.Log(LogLevel.Warning, e, "{0} was canceled", request);
			}
			catch (Exception e)
			{
				_logger.Log(LogLevel.Error, e, "{0} crashed with message: {1}", request, e.Message);
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