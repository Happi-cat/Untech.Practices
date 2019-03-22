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
		private const int SlidingCoefficientRadius = 5;

		private readonly ITransport _transport;
		private readonly RequestProcessor _requestProcessor;
		private readonly RequestMetadataAccessors _requestMetadataAccessors;
		private readonly object _sync = new object();

		private readonly List<Slot> _slots;

		private int _ticks = 0;
		private int _slidingCoefficient = 0;
		private CancellationTokenSource _aborted;
		private Timer _timer;

		public Orchestrator(ITransport transport, RequestMetadataAccessors requestMetadataAccessors,
			RequestProcessor requestProcessor)
		{
			_transport = transport;
			_requestProcessor = requestProcessor;
			_requestMetadataAccessors = requestMetadataAccessors;

			_slots = Enumerable.Range(0, 5).Select(n => new Slot()).ToList();
		}

		public Task StartAsync()
		{
			_timer = new Timer(OnTimer, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
			_aborted = new CancellationTokenSource();

			return Task.CompletedTask;
		}

		public Task StopAsync(TimeSpan delay)
		{
			_timer.Dispose();
			_aborted.CancelAfter(delay);

			return Task.WhenAll(_slots.Select(s => s.Task));
		}

		private void OnTimer(object state)
		{
			lock (_sync)
			{
				if (_ticks <= 0)
				{
					var slot = _slots.FirstOrDefault(n => n.CanTake());
					slot?.Take(Do);

					_ticks = GetTicksFromSlidingCoefficient();
				}
				else
				{
					_ticks--;
				}
			}

			int GetTicksFromSlidingCoefficient()
			{
				var c = _slidingCoefficient + SlidingCoefficientRadius;
				return Math.Max(0, Math.Min(c, SlidingCoefficientRadius * 2));
			}
		}

		private async Task Do()
		{
			var requests = await _transport.GetRequestsAsync(10);

			UpdateSlidingCoefficient();

			await Task.WhenAll(requests.Select(Do));

			void UpdateSlidingCoefficient()
			{
				var l = requests.Length;
				var c = _slidingCoefficient;
				const int maxC = SlidingCoefficientRadius * 2;

				if (l <= 2 && c < maxC)
				{
					Interlocked.Increment(ref _slidingCoefficient);
				}
				else if (l >= 8 && -maxC < c)
				{
					Interlocked.Decrement(ref _slidingCoefficient);
				}
			}
		}

		private Task Do(Request request)
		{
			var context = new Context(request, _requestMetadataAccessors.GetMetadata(request.Name))
			{
				Aborted = _aborted.Token
			};

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
			public Task Task = Task.CompletedTask;

			public bool CanTake()
			{
				return Task.Status == TaskStatus.RanToCompletion || Task.Status == TaskStatus.Faulted;
			}

			public void Take(Func<Task> function)
			{
				if (!CanTake()) throw new InvalidOperationException();

				Task = Task.Run(function);
			}
		}
	}
}