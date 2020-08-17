using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Untech.AsyncJob.Processing
{
	public class RequestProcessor : IRequestProcessor
	{
		private readonly RequestProcessorCallback _next;

		public RequestProcessor(IEnumerable<IRequestProcessorMiddleware> middlewares)
			: this(middlewares, new NotHandledRequestProcessor())
		{
		}

		public RequestProcessor(IEnumerable<IRequestProcessorMiddleware> middlewares, IRequestProcessor endware)
		{
			if (middlewares == null) throw new ArgumentNullException(nameof(middlewares));
			if (endware == null) throw new ArgumentNullException(nameof(endware));

			_next = BuildChain(new Queue<IRequestProcessorMiddleware>(middlewares), endware);
		}

		public Task InvokeAsync(Context context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			return _next(context);
		}

		private static RequestProcessorCallback BuildChain(
			Queue<IRequestProcessorMiddleware> middlewares,
			IRequestProcessor endware)
		{
			if (middlewares.Count <= 0) return endware.InvokeAsync;

			var currentMiddleware = middlewares.Dequeue();
			var next = BuildChain(middlewares, endware);

			return currentMiddleware != null
				? ctx => currentMiddleware.InvokeAsync(ctx, next)
				: next;
		}

		private class NotHandledRequestProcessor : IRequestProcessor
		{
			public Task InvokeAsync(Context context)
			{
				throw new InvalidOperationException("Request wasn't handled in previous middleware.");
			}
		}
	}
}
