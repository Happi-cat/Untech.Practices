using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine.Processing
{
	public class RequestProcessor : IRequestProcessor
	{
		private static readonly RequestProcessorCallback s_defaultNext = ctx => Task.FromResult(0);

		private readonly RequestProcessorCallback _next;

		public RequestProcessor(IEnumerable<IRequestProcessorMiddleware> steps)
		{
			_next = BuildChain(steps);
		}

		public Task InvokeAsync(Context context)
		{
			return _next(context);
		}

		private static RequestProcessorCallback BuildChain(IEnumerable<IRequestProcessorMiddleware> steps)
		{
			steps = steps ?? Enumerable.Empty<IRequestProcessorMiddleware>();
			return BuildChain(new Queue<IRequestProcessorMiddleware>(steps));
		}

		private static RequestProcessorCallback BuildChain(Queue<IRequestProcessorMiddleware> steps)
		{
			if (steps.Count > 0)
			{
				var currentMiddleware = steps.Dequeue();
				var next = BuildChain(steps);

				return ctx =>
				{
					Console.WriteLine("{0}:{1}: {2}", ctx.TraceIdentifier, DateTime.UtcNow.Ticks, currentMiddleware.GetType().Name);
					return currentMiddleware.InvokeAsync(ctx, next);
				};
			}

			return s_defaultNext;
		}
	}
}