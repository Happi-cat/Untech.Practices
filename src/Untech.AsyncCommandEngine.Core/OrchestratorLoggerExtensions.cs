using System;
using Microsoft.Extensions.Logging;

namespace Untech.AsyncCommandEngine
{
	internal static class OrchestratorLoggerExtensions
	{
		private static readonly Action<ILogger, int, Exception> s_tickSkipped;
		private static readonly Action<ILogger, int, Exception> s_slidingCoefficientIncreased;
		private static readonly Action<ILogger, int, Exception> s_slidingCoefficientDecreased;
		private static readonly Action<ILogger, Exception> s_freeWarpAvailable;
		private static readonly Action<ILogger, Exception> s_noFreeWarpAvailable;
		private static readonly Action<ILogger, Exception> s_warpCrashed;

		static OrchestratorLoggerExtensions()
		{
			s_tickSkipped = LoggerMessage.Define<int>(LogLevel.Information,
				new EventId(1, nameof(TickSkipped)),
				"Tick skipped, will be reached after {ticksRemain}");

			s_slidingCoefficientIncreased = LoggerMessage.Define<int>(LogLevel.Information,
				new EventId(2, nameof(SlidingCoefficientIncreased)),
				"Sliding coefficient was increased to {coefficient}");

			s_slidingCoefficientDecreased = LoggerMessage.Define<int>(LogLevel.Information,
				new EventId(3, nameof(SlidingCoefficientDecreased)),
				"Sliding coefficient was decreased to {coefficient}");

			s_freeWarpAvailable = LoggerMessage.Define(LogLevel.Information,
				new EventId(4, nameof(IsFreeWarpAvailable)),
				"One of warps is available");

			s_noFreeWarpAvailable = LoggerMessage.Define(LogLevel.Information,
				new EventId(5, nameof(IsNoFreeWarpAvailable)),
				"No available warps");

			s_warpCrashed = LoggerMessage.Define(LogLevel.Error,
				new EventId(6, "Warp Crashed"),
				"Error occured during warp execution");
		}

		public static void TickSkipped(this ILogger logger, int ticksRemain)
		{
			s_tickSkipped(logger, ticksRemain, null);
		}

		public static void SlidingCoefficientIncreased(this ILogger logger, int coefficient)
		{
			s_slidingCoefficientIncreased(logger, coefficient, null);
		}

		public static void SlidingCoefficientDecreased(this ILogger logger, int coefficient)
		{
			s_slidingCoefficientDecreased(logger, coefficient, null);
		}

		public static void IsFreeWarpAvailable(this ILogger logger)
		{
			s_freeWarpAvailable(logger, null);
		}

		public static void IsNoFreeWarpAvailable(this ILogger logger)
		{
			s_noFreeWarpAvailable(logger, null);
		}

		public static void WarpCrashed(this ILogger logger, Exception e)
		{
			s_warpCrashed(logger, e);
		}

	}
}