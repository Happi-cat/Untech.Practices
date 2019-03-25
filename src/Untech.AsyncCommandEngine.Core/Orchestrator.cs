using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.AsyncCommandEngine.Metadata;
using Untech.AsyncCommandEngine.Processing;

namespace Untech.AsyncCommandEngine
{
	public class OrchestratorOptions
	{
		public int Warps { get; set; }
		public int RequestsPerWarp { get; set; }
		public int SlidingRadius { get; set; }
		public TimeSpan SlidingStep { get; set; }
	}

	public class Orchestrator : IOrchestrator
	{
		private readonly OrchestratorOptions _options;
		private readonly ITransport _transport;
		private readonly IRequestProcessor _requestProcessor;
		private readonly IRequestMetadataAccessors _metadataAccessors;
		private readonly object _sync = new object();

		private readonly List<Warp> _warps;

		private CancellationTokenSource _aborted;
		private SlidingTimer _timer;

		public Orchestrator(OrchestratorOptions options,
			ITransport transport,
			IRequestMetadataAccessors metadataAccessors,
			IRequestProcessor requestProcessor)
		{
			_options = options;
			_transport = transport;
			_requestProcessor = requestProcessor;
			_metadataAccessors = metadataAccessors;

			_warps = Enumerable.Range(0, options.Warps).Select(n => new Warp()).ToList();
		}

		public Task StartAsync()
		{
			_timer = new SlidingTimer(OnTimer, _options.SlidingStep, _options.SlidingRadius);
			_aborted = new CancellationTokenSource();

			return Task.CompletedTask;
		}

		public Task StopAsync(TimeSpan delay)
		{
			_timer.Dispose();
			_aborted.CancelAfter(delay);

			return Task.WhenAll(_warps.Select(s => s.Task));
		}

		private void OnTimer()
		{
			lock (_sync)
			{
				var slot = _warps.FirstOrDefault(n => n.CanRun());
				slot?.Run(Do);
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
			var context = new Context(request, _metadataAccessors.GetMetadata(request.Name))
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

		private class Warp
		{
			private readonly object _sync = new object();

			private Task _task = Task.CompletedTask;

			public Task Task => _task;

			public bool CanRun()
			{
				return _task.Status == TaskStatus.RanToCompletion
					|| _task.Status == TaskStatus.Faulted;
			}

			public void Run(Func<Task> function)
			{
				lock (_sync)
				{
					if (!CanRun()) throw new InvalidOperationException("Warp is busy");

					_task = Task.Run(function);
				}
			}
		}

		private class SlidingTimer : IDisposable
		{
			private readonly Timer _timer;
			private readonly Action _timerCallback;

			private readonly int _slidingRadius;

			private int _slidingCoefficient = 0;
			private int _ticksRemain = 0;

			public SlidingTimer(Action callback, TimeSpan step, int slidingRadius)
			{
				_timerCallback = callback;
				_slidingRadius = slidingRadius;
				_timer = new Timer(OnTick, null, TimeSpan.Zero, step);
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