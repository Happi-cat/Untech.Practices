using System;

namespace Untech.Practices.CQRS.Dispatching
{
	public class QueueOptions
	{
		public static readonly QueueOptions Default = new QueueOptions();

		public QueueOptions()
		{
			TimeToLive = 5;
		}

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
		public int TimeToLive { get; set; }

		/// <summary>
		/// Gets or sets request priority. Max - is the highest priority, min represents lowest priority.
		/// </summary>
		public int Priority { get; set; }
	}
}