using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.ExpenseLog.MonthLog;
using Untech.Practices;
using Untech.Practices.CQRS;
using Untech.Practices.DataStorage;

namespace MyBudgetPlan.Domain.ExpenseLog.Forecast
{
	[DataContract]
	public class ProjectedExpense : AggregateRoot
	{
		private Money _amount;

		private ProjectedExpense()
		{

		}

		public ProjectedExpense(int key, string categoryKey, YearMonth when, Money amount, string description = null)
		  :base(key)
		{
			CategoryKey = categoryKey;
			When = when;
			Amount = amount;
			Description = description;
		}

		[DataMember]
		public string CategoryKey { get; private set; }

		[DataMember]
		public YearMonth When { get; private set; }

		[DataMember]
		public Money Amount
		{
			get => _amount;
			private set
			{
				if (value.Amount < 0) throw new ArgumentException("Amount cannot be negative");
				_amount = value;
			}
		}

		[DataMember]
		public string Description { get; set; }

		public void UpdateAmount(Money amount)
		{
			if (amount.Amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));

			Amount = amount;
			Raise(new ExpenseMonthLogChanged(When));
		}

		public void UpdateCategory(string categoryKey)
		{
			var oldValue = CategoryKey;
			CategoryKey = categoryKey;

			var shouldRaise = Key != 0 && oldValue != categoryKey;
			if (shouldRaise)
			{
				Raise(new ExpenseMonthLogChanged(When));
			}
		}

		public void UpdateDescription(string description)
		{
			Description = description;
		}
	}
}