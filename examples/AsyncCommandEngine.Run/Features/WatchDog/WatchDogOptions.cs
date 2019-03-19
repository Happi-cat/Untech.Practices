using System;
using System.Collections.ObjectModel;

namespace AsyncCommandEngine.Run
{
	public class WatchDogOptions
	{
		/// <summary>
		/// Null is for disabled default timeout
		/// </summary>
		public TimeSpan? DefaultTimeout { get; set; }

		public ReadOnlyDictionary<string, TimeSpan> RequestTimeouts { get; set; }
	}
}