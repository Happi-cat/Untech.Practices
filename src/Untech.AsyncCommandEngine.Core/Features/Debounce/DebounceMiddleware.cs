using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Metadata.Annotations;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine.Features.Debounce
{
	internal class DebounceMiddleware : IRequestProcessorMiddleware
	{
		private readonly ILastRunStore _lastRunStore;
		private readonly ILogger _logger;

		public DebounceMiddleware(ILastRunStore lastRunStore, ILoggerFactory loggerFactory)
		{
			_lastRunStore = lastRunStore;
			_logger = loggerFactory.CreateLogger<DebounceMiddleware>();
		}

		public async Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			var debounceAttribute = context.RequestMetadata.GetAttribute<DebounceAttribute>();

			if (debounceAttribute != null)
			{
				var lastRun = await _lastRunStore.GetLastRunAsync(context.Request, context.Aborted);
				if (lastRun != null && context.Request.Created < lastRun)
				{
					_logger.RequestDebounced(context);
					return;
				}

				await next(context);
				await _lastRunStore.SetLastRunAsync(context.Request);
			}
			else
			{
				await next(context);
			}
		}
	}
}