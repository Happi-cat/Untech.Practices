using System;

namespace Untech.AsyncCommandEngine.Features.WatchDog
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class WatchDogTimeoutAttribute : Attribute
	{
		private readonly int _hours;
		private readonly int _minutes;
		private readonly int _seconds;

		public WatchDogTimeoutAttribute(int hours, int minutes, int seconds)
		{
			_hours = hours;
			_minutes = minutes;
			_seconds = seconds;
		}

		public TimeSpan Timeout => new TimeSpan(_hours, _minutes, _seconds);
	}
}