using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	public class RequestProcessor
	{
		private static readonly RequestProcessorCallback s_defaultNext = ctx => Task.FromResult(0);

		private readonly RequestProcessorCallback _next;

		public RequestProcessor(IEnumerable<IRequestProcessorMiddleware> middlewares)
		{
			_next = BuildChain(middlewares);
		}

		public Task ExecuteAsync(Context context)
		{
			return _next(context);
		}

		private static RequestProcessorCallback BuildChain(IEnumerable<IRequestProcessorMiddleware> middlewares)
		{
			middlewares = middlewares ?? Enumerable.Empty<IRequestProcessorMiddleware>();
			return BuildChain(new Queue<IRequestProcessorMiddleware>(middlewares));
		}

		private static RequestProcessorCallback BuildChain(Queue<IRequestProcessorMiddleware> middlewares)
		{
			if (middlewares.Count > 0)
			{
				var currentMiddleware = middlewares.Dequeue();
				var next = BuildChain(middlewares);

				return ctx =>
				{
					Console.WriteLine(currentMiddleware.GetType().FullName);
					return currentMiddleware.ExecuteAsync(ctx, next);
				};
			}

			return s_defaultNext;
		}
	}
}