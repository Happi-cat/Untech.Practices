using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Untech.AsyncJob.Features.Debounce;
using Untech.AsyncJob.Formatting;
using Untech.AsyncJob.Processing;

namespace Untech.AsyncJob.Features.Formatting
{
	internal class FormattingMiddleware : IRequestProcessorMiddleware
	{
		private readonly IReadOnlyDictionary<string, IRequestContentFormatter> _formatters;
		private readonly IRequestContentFormatter _fallbackFormatter;

		public FormattingMiddleware(IEnumerable<IRequestContentFormatter> formatters, IRequestContentFormatter fallbackFormatter)
		{
			if (_formatters == null) throw new ArgumentNullException(nameof(formatters));
			if (_fallbackFormatter == null) throw new ArgumentNullException(nameof(fallbackFormatter));

			_formatters = formatters.ToDictionary(f => f.Type, f => f);
			_fallbackFormatter = fallbackFormatter;
		}

		public Task InvokeAsync(Context context, RequestProcessorCallback next)
		{
			var formatter = ResolveFormatter(context.Request.ContentType);

			context.Request.Items[typeof(IRequestContentFormatter)] = formatter;
			context.Items[typeof(IRequestContentFormatter)] = formatter;

			return next(context);
		}

		private IRequestContentFormatter ResolveFormatter(string contentType)
		{
			if (contentType != null && _formatters.TryGetValue(contentType, out var f))
				return f;

			return _fallbackFormatter;
		}
	}
}