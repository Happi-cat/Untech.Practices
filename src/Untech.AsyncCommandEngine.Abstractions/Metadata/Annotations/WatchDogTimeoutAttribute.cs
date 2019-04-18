using System;

namespace Untech.AsyncCommandEngine.Metadata.Annotations
{
	/// <summary>
	/// Sets the flag that current request should be controlled by WatchDog and should be cancel after <see cref="Timeout"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class WatchDogTimeoutAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WatchDogTimeoutAttribute"/> with the defined seconds.
		/// </summary>
		/// <param name="seconds">The amount of seconds.</param>
		public WatchDogTimeoutAttribute(int seconds) : this(0 , 0, seconds)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WatchDogTimeoutAttribute"/> with the defined minutes, seconds.
		/// </summary>
		/// <param name="minutes">The amount of minutes.</param>
		/// <param name="seconds">The amount of seconds.</param>
		public WatchDogTimeoutAttribute(int minutes, int seconds) : this(0 ,minutes, seconds)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WatchDogTimeoutAttribute"/> with the defined hours, minutes, seconds.
		/// </summary>
		/// <param name="hours">The amount of hours.</param>
		/// <param name="minutes">The amount of minutes.</param>
		/// <param name="seconds">The amount of seconds.</param>
		public WatchDogTimeoutAttribute(int hours, int minutes, int seconds)
		{
			Hours = hours;
			Minutes = minutes;
			Seconds = seconds;
		}

		/// <summary>
		/// Gets the amount of hours.
		/// </summary>
		public int Hours { get; private set; }
		/// <summary>
		/// Gets the amount of minutes.
		/// </summary>
		public int Minutes { get; private set; }

		/// <summary>
		/// Gets the amount of seconds.
		/// </summary>
		public int Seconds { get; private set; }

		/// <summary>
		/// Gets the defined timeout after which request should be cancelled.
		/// </summary>
		public TimeSpan Timeout => new TimeSpan(Hours, Minutes, Seconds);
	}
}