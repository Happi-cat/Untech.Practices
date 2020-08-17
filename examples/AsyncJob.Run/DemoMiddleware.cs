using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob;
using Untech.AsyncJob.Metadata.Annotations;
using Untech.AsyncJob.Processing;

namespace AsyncJob.Run
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
				TraceId = context.TraceIdentifier,
				Name = context.RequestName.Substring(context.RequestName.LastIndexOf('.') + 1),
				Body = GetBodyAsJson(context.Request)
			};

			using (_logger.BeginScope(requestScope))
			{
				var opts = context.RequestMetadata.GetAttributes<OptionAttribute>()
					.Select(attr => (attr.Key, attr.Value))
					.ToList();

				_logger.Log(LogLevel.Debug, "starting with opts: {0}", opts);
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
					_logger.Log(LogLevel.Error, "crashed: {0}", e.Message);
				}
			}
		}

		private static string GetBodyAsJson(Request request)
		{
			return request.Content;
		}
	}
}
