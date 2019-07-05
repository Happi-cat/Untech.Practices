using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using Untech.Practices;
using Untech.Practices.CQRS;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Domain
{
	[DataContract]
	public abstract class BudgetLogEntry : IHasKey
	{
		private readonly List<INotification> _notifications = new List<INotification>();

		private Money _amount;

		protected BudgetLogEntry()
		{
		}

		protected BudgetLogEntry(int key, BudgetLogType log)
		{
			Key = key;
			Log = log;
		}

		[IgnoreDataMember]
		public IReadOnlyList<INotification> NotificationsToRaise => _notifications;

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public BudgetLogType Log { get; private set; }

		[DataMember]
		public string CategoryKey { get; protected set; }

		[DataMember]
		public Money Amount
		{
			get => _amount;
			protected set
			{
				if (value.Amount < 0) throw new ArgumentException("Amount cannot be negative");
				_amount = value;
			}
		}

		[DataMember]
		public string Description { get; protected set; }

		protected void Raise(INotification notification)
		{
			_notifications.Add(notification);
		}
	}
}