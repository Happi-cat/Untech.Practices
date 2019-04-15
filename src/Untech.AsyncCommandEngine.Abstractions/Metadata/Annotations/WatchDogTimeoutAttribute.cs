using System;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class WatchDogTimeoutAttribute : Attribute
	{
		public WatchDogTimeoutAttribute(int hours, int minutes, int seconds)
		{
			Hours = hours;
			Minutes = minutes;
			Seconds = seconds;
		}

		public int Hours { get; }
		public int Minutes { get; }
		public int Seconds { get; }

		public TimeSpan Timeout => new TimeSpan(Hours, Minutes, Seconds);
	}
}