using System;

namespace Untech.AsyncCommandEngine
{
	public class OrchestratorOptions
	{
		public int Warps { get; set; }
		public int RequestsPerWarp { get; set; }

		public int SlidingRadius { get; set; }
		public TimeSpan SlidingStep { get; set; }

		public bool RunRequestsAllAtOnceInWarp { get; set; }
	}
}