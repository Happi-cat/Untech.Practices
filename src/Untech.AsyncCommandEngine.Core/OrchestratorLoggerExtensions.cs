using Microsoft.Extensions.Logging;

namespace Untech.AsyncCommandEngine
{
	internal static class OrchestratorLoggerExtensions
	{
		public static void TickSkipped(this ILogger logger, int ticksRemain)
		{
		}

		public static void SlidingCoefficientIncreased(this ILogger logger, int coefficient)
		{
		}

		public static void SlidingCoefficientDecreased(this ILogger logger, int coefficient)
		{
		}

		public static void IsFreeWarpAvailable(this ILogger logger)
		{
		}

		public static void NoFreeWarpAvailable(this ILogger logger)
		{
		}
	}
}