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

		public int Key { get; private set; }

		[IgnoreDataMember]
		public IReadOnlyList<INotification> NotificationsToRaise => _notifications;

		protected void Raise(INotification notification)
		{
			_notifications.Add(notification);
		}
	}
}