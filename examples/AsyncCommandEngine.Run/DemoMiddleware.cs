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

			var requestScope = new
			{
				TraceId= context.TraceIdentifier,
				Name = context.RequestName.Substring(context.RequestName.LastIndexOf('.') + 1),
				Body = GetBodyAsJson(context.Request)
			};

			using (_logger.BeginScope(requestScope))
			{
				_logger.Log(LogLevel.Debug, "starting");
				try
				{
					await next(context);
				}
				catch (OperationCanceledException e)
				{
					_logger.Log(LogLevel.Warning, e, "canceled");
				}
				catch (Exception e)
				{
					_logger.Log(LogLevel.Error, e, "crashed: {0}", e.Message);
				}
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