using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Untech.AsyncCommmandEngine.Abstractions;

namespace AsyncCommandEngine.Run
{
	internal class AceProcessor
	{
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
			if (middlewares.TryDequeue(out IAceProcessorMiddleware middleware))
			{
				var next = GetNext(middlewares);

				return ctx =>
				{
					Console.WriteLine("Middleware: {0}", middleware.GetType());
					return middleware.Execute(ctx, next);
				};
			}

			return ctx => Task.CompletedTask;
		}
	}
}