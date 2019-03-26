using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Untech.AsyncCommandEngine
{
	public partial class Orchestrator
	{
		private class SlidingTimer : IDisposable
		{
			private readonly Timer _timer;
			private readonly Action _timerCallback;

			private readonly int _slidingRadius;
			private readonly ILogger _logger;

			private int _slidingCoefficient = 0;
			private int _ticksRemain = 0;

			public SlidingTimer(Action callback, TimeSpan step, int slidingRadius, ILogger logger)
			{
				_timerCallback = callback;
				_slidingRadius = slidingRadius;
				_logger = logger;
				_timer = new Timer(OnTick, null, TimeSpan.Zero, step);
			}

			public void Increase()
			{
				var c = _slidingCoefficient;
				var r = _slidingRadius;

				// 2r - adds some laziness
				if (c >= 2 * r) return;

				Interlocked.Increment(ref _slidingCoefficient);
				_logger.SlidingCoefficientIncreased(_slidingCoefficient);
			}

			public void Decrease()
			{
				var c = _slidingCoefficient;
				var r = _slidingRadius;

				// 2r - adds some laziness
				if (c <= -2 * r) return;

				Interlocked.Decrement(ref _slidingCoefficient);
				_logger.SlidingCoefficientDecreased(_slidingCoefficient);
			}

			private void OnTick(object state)
			{
				if (_ticksRemain <= 0)
				{
					Interlocked.Exchange(ref _ticksRemain, GetTicksFromSlidingCoefficient());
					_timerCallback();
				}
				else
				{
					Interlocked.Decrement(ref _ticksRemain);
					_logger.TickSkipped(_ticksRemain);
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