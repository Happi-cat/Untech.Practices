using System;
using System.Collections.Generic;
using System.Linq;
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
}