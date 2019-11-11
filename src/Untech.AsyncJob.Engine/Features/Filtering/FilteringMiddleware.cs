using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.Filtering
{
	internal class FilteringMiddleware : IRequestProcessorMiddleware
	{
		private readonly ILogger _logger;
		private readonly Func<Request, bool> _predicate;

		public FilteringMiddleware(ILoggerFactory loggerFactory, Func<Request, bool> predicate)
		{
			_logger = loggerFactory.CreateLogger<FilteringMiddleware>();
			_predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
		}

		public Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			if (_predicate(context.Request))
				return next(context);

			_logger.RequestFiltered(context);
			return Task.CompletedTask;
		}
	}
}