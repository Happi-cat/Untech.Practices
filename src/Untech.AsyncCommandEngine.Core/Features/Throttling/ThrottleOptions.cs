using System.Collections.ObjectModel;

namespace Untech.AsyncCommandEngine.Features.Throttling
{
	public class ThrottleOptions
	{
		public int? RunAtOnce { get; set; }

		public int? DefaultRunAtOnceInGroup { get; set; }

		public ReadOnlyDictionary<string, ThrottleGroupOptions> Groups { get; set; }
	}
}