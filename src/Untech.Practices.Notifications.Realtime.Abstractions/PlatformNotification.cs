using System;

namespace Untech.Practices.Notifications.Realtime
{
	/// <summary>
	/// Represents plaform-wide realtime notification.
	/// </summary>
	public class PlatformNotification : IRealtimeNotification
	{
		public PlatformNotification(object payload)
		{
			Payload = payload ?? throw new ArgumentNullException(nameof(payload));
		}

		public object Payload { get; }
	}
}