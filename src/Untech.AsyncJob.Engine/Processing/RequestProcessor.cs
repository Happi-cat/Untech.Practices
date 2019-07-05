using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Untech.AsyncJob.Processing
{
	internal class RequestProcessor : IRequestProcessor
	{
		private readonly RequestProcessorCallback _next;

		public RequestProcessor(IEnumerable<IRequestProcessorMiddleware> steps)
		{
			if (steps == null) throw new ArgumentNullException(nameof(steps));

			_next = BuildChain(new Queue<IRequestProcessorMiddleware>(steps));
		}

		public Task InvokeAsync(Context context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			return _next(context);
		}

		private static RequestProcessorCallback BuildChain(Queue<IRequestProcessorMiddleware> steps)
		{
			if (steps.Count <= 0) return DoFinally;

			var currentMiddleware = steps.Dequeue();
			var next = BuildChain(steps);

			return currentMiddleware != null
				? ctx => currentMiddleware.InvokeAsync(ctx, next)
				: next;
		}

		private static Task DoFinally(Context ctx)
		{
			throw new InvalidOperationException("Request wasn't handled in previous middleware.");
		}
	}
}
