using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Untech.AsyncCommandEngine
{
	public class Orchestrator : IOrchestrator
	{
		private readonly ITransport _transport;
		private readonly RequestProcessor _requestProcessor;
		private readonly RequestMetadataAccessors _requestMetadataAccessors;
		private readonly Timer _timer;
		private readonly List<Slot> _slots;

		private readonly object _sync = new object();

		public Orchestrator(ITransport transport, RequestProcessor requestProcessor)
		{
			_transport = transport;
			_requestProcessor = requestProcessor;
		}

		public async Task StartAsync()
		{
			throw new NotImplementedException();
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		private void OnTimer()
		{
			if (Monitor.IsEntered(_sync)) return;

			lock (_sync)
			{
				var availableSlots = _slots.Where(slot => slot.CanTake()).ToList();

				foreach (var slot in availableSlots)
				{
					slot.Take(Do);
				}
			}
		}

		private async Task Do()
		{
			var request = await _transport.GetRequestAsync();

			await Do(request);
		}

		private Task Do(Request request)
		{
			var context = new Context(request, _requestMetadataAccessors.GetMetadata(request.Name));

			return Do(context);
		}

		private async Task Do(Context context)
		{
			try
			{
				await _requestProcessor.InvokeAsync(context);
			}
			catch (Exception exception)
			{
				await _transport.FailRequestAsync(context.Request, exception);
				return;
			}

			await _transport.CompleteRequestAsync(context.Request);
		}

		private class Slot
		{
			private Task _task;

			public bool CanTake()
			{
				return _task.Status == TaskStatus.RanToCompletion || _task.Status == TaskStatus.Faulted;
			}

			public void Take(Func<Task> function)
			{
				if (!CanTake()) throw new InvalidOperationException();

				_task = Task.Run(function);
			}
		}
	}

	public class RequestProcessor
	{
		private static readonly RequestProcessorCallback s_defaultNext = ctx => Task.FromResult(0);

		private readonly RequestProcessorCallback _next;

		public RequestProcessor(IEnumerable<IRequestProcessorMiddleware> middlewares)
		{
			_next = BuildChain(middlewares);
		}

		public Task InvokeAsync(Context context)
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
					Console.WriteLine("{0}:{1}: {2}", ctx.TraceIdentifier, DateTime.UtcNow.Ticks, currentMiddleware.GetType().Name);
					return currentMiddleware.InvokeAsync(ctx, next);
				};
			}

			return s_defaultNext;
		}
	}
}