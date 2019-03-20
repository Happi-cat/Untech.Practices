using System.Collections.Generic;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	public class AceProcessor
	{
		private static readonly AceRequestProcessorDelegate _defaultNext = ctx => Task.FromResult(0);

		private readonly AceRequestProcessorDelegate _next;

		public AceProcessor(IEnumerable<IAceProcessorMiddleware> middlewares)
		{
			_next = GetNext(new Queue<IAceProcessorMiddleware>(middlewares ?? new List<IAceProcessorMiddleware>()));
		}

		public Task Execute(AceContext context)
		{
			return _next(context);
		}

		private static AceRequestProcessorDelegate GetNext(Queue<IAceProcessorMiddleware> middlewares)
		{
			if (middlewares.Count > 0)
			{
				var middleware = middlewares.Dequeue();
				var next = GetNext(middlewares);

				return ctx => middleware.ExecuteAsync(ctx, next);
			}

			return _defaultNext;
		}
	}
}