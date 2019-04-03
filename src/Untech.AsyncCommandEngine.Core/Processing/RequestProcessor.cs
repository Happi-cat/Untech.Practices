using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Processing
{
	internal class RequestProcessor : IRequestProcessor
	{
		private static readonly RequestProcessorCallback s_defaultNext = ctx => Task.FromResult(0);

		private readonly RequestProcessorCallback _next;

		public RequestProcessor(IEnumerable<IRequestProcessorMiddleware> steps)
		{
			if (steps == null) throw new ArgumentNullException(nameof(steps));

			_next = BuildChain(new Queue<IRequestProcessorMiddleware>(steps));
		}

		public Task InvokeAsync(Context context)
		{
			return _next(context);
		}

		private static RequestProcessorCallback BuildChain(Queue<IRequestProcessorMiddleware> steps)
		{
			if (steps.Count <= 0) return s_defaultNext;

			var currentMiddleware = steps.Dequeue();
			var next = BuildChain(steps);

			return currentMiddleware != null
				? ctx => currentMiddleware.InvokeAsync(ctx, next)
				: next;
		}
	}
}