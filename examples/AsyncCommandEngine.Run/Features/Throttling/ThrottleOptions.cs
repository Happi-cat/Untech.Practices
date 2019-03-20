using System.Collections.ObjectModel;

namespace AsyncCommandEngine.Examples.Features.Throttling
{
	public class ThrottleOptions
	{
		public int? DefaultRunAtOnce { get; set; }
		public int? DefaultRunAtOnceInGroup { get; set; }

		public ReadOnlyDictionary<string, ThrottleGroupOptions> Groups { get; set; }
	}
}