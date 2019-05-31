using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using static System.Math;

namespace Untech.AsyncCommandEngine
{
	internal partial class Orchestrator
	{
		private class SlidingTimer : IDisposable
		{
			private readonly Timer _timer;
			private readonly Action _timerCallback;

			private readonly int _slidingRadius;
			private readonly ILogger _logger;

			private int _slidingCoefficient;
			private int _ticksRemain;

			public SlidingTimer(Action callback, TimeSpan step, int slidingRadius, ILogger logger)
			{
				if (step <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(step));
				if (slidingRadius < 0) throw new ArgumentOutOfRangeException(nameof(slidingRadius));

				_timerCallback = callback;
				_slidingRadius = slidingRadius;
				_logger = logger;
				_timer = new Timer(OnTick, null, TimeSpan.Zero, step);
			}

			public float GetSlidingPercentage()
			{
				var r = _slidingRadius;
				var c = _slidingCoefficient;

				return (float)(c + r) / (2 * r);
			}

			public void Increase()
			{
				var r = _slidingRadius;
				var c = _slidingCoefficient;

				// 2r - adds some laziness
				if (r == 0 || 2 * r <= c) return;

				Interlocked.Increment(ref _slidingCoefficient);
				_logger.SlidingCoefficientIncreased(_slidingCoefficient);
			}

			public void Decrease()
			{
				var r = _slidingRadius;
				var c = _slidingCoefficient;

				// 2r - adds some laziness
				if (r == 0 || c <= -2 * r) return;

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

				if (r == 0) return 0;

				return Max(-r, Min(c, r)) + r;
			}

			public void Dispose()
			{
				_timer.Dispose();
			}
		}
	}
}