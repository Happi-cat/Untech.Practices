using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Domain
{
	[DataContract]
	public abstract class AggregateRoot : IAggregateRoot
	{
		private readonly List<INotification> _notifications = new List<INotification>();

		protected AggregateRoot()
		{
		}

		protected AggregateRoot(int key)
		{
			Key = key;
		}

		[IgnoreDataMember]
		public IReadOnlyList<INotification> NotificationsToRaise => _notifications;

		[DataMember]
		public int Key { get; }

		protected void Raise(INotification notification)
		{
			_notifications.Add(notification);
		}
	}
}