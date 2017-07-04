using System;

namespace Untech.Practices.CQRS
{
	/// <summary>
	/// Represents class that contains different options for queuing of CQRS requests.
	/// </summary>
	public class QueueOptions
	{
		/// <summary>
		/// Default time to live: 5.
		/// </summary>
		public const int DefaultTimeToLive = 5;

		/// <summary>
		/// Gets or sets execution delay timeout.
		/// </summary>
		public TimeSpan? ExecuteAfter { get; set; }

		/// <summary>
		/// Gets or sets life time.
		/// </summary>
		public TimeSpan? ExpiresAfter { get; set; }

		/// <summary>
		/// Gets or sets number of replies on failures.
		/// </summary>
		public int TimeToLive { get; set; } = DefaultTimeToLive;

		/// <summary>
		/// Gets or sets request priority. Max - is the highest priority, min represents lowest priority.
		/// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// Creates default <see cref="QueueOptions"/>.
		/// </summary>
		/// <returns></returns>
		public static QueueOptions CreateDefault() => new QueueOptions
		{
			TimeToLive = DefaultTimeToLive
		};
	}
}