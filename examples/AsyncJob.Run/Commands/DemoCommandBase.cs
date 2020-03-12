using System;
using System.Collections.Generic;
using Untech.AsyncJob.Metadata.Annotations;

namespace AsyncJob.Run.Commands
{
	public class DemoCommandBase
	{
		public TimeSpan? WatchDogTimeout { get; set; }

		public IEnumerable<MetadataAttribute> GetMetadata()
		{
			if (WatchDogTimeout != null)
			{
				var timeout = WatchDogTimeout.Value;
				yield return new WatchDogTimeoutAttribute(timeout.Hours, timeout.Minutes, timeout.Seconds);
			}
		}
	}
}