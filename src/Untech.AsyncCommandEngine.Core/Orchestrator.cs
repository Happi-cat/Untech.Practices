using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		private readonly object _sync = new object();

		private readonly List<Slot> _slots;

		private CancellationTokenSource _aborted;
		private SlidingTimer _timer;

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
			_timer = new SlidingTimer(OnTimer, TimeSpan.FromSeconds(10), 6);
			_aborted = new CancellationTokenSource();

			return Task.CompletedTask;
		}

		public Task StopAsync(TimeSpan delay)
		{
			_timer.Dispose();
			_aborted.CancelAfter(delay);

			return Task.WhenAll(_slots.Select(s => s.Task));
		}

		private void OnTimer()
		{
			lock (_sync)
			{
				var slot = _slots.FirstOrDefault(n => n.CanTake());
				slot?.Take(Do);
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

				if (l <= 2)
				{
					_timer.SlideUp();
				}
				else if (l >= 8)
				{
					_timer.SlideDown();
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

		private class SlidingTimer : IDisposable
		{
			private readonly Timer _timer;
			private readonly Action _timerCallback;

			private readonly int _slidingRadius;

			private int _slidingCoefficient = 0;
			private int _ticksRemain = 0;

			public SlidingTimer(Action callback, TimeSpan minPeriod, int slidingRadius)
			{
				_timerCallback = callback;
				_slidingRadius = slidingRadius;
				_timer = new Timer(OnTick, null, TimeSpan.Zero, minPeriod);
			}

			public void SlideUp()
			{
				var c = _slidingCoefficient;
				var r = _slidingRadius;

				// 2r - adds some laziness
				if (c < 2 * r)
					Interlocked.Increment(ref _slidingCoefficient);
			}

			public void SlideDown()
			{
				var c = _slidingCoefficient;
				var r = _slidingRadius;

				// 2r - adds some laziness
				if (-2 * r < c)
					Interlocked.Decrement(ref _slidingCoefficient);
			}

			private void OnTick(object state)
			{
				if (_ticksRemain <= 0)
				{
					_timerCallback();
					Interlocked.Exchange(ref _ticksRemain, GetTicksFromSlidingCoefficient());
				}
				else
				{
					Interlocked.Decrement(ref _ticksRemain);
				}
			}

			private int GetTicksFromSlidingCoefficient()
			{
				var r = _slidingRadius;
				var c = _slidingCoefficient;

				return Math.Max(-r, Math.Min(c, r)) + r;
			}

			public void Dispose()
			{
				_timer.Dispose();
			}
		}
	}
}