using System;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.ExpenseLog.MonthLog;
using NodaTime;
using Untech.Practices;

namespace MyBudgetPlan.Domain.ExpenseLog.Actual
{
	[DataContract]
	public class ActualExpense : AggregateRoot
	{
		private Money _amount;

		private ActualExpense()
		{
		}

		public ActualExpense(int key, string categoryKey, LocalDate when, Money amount, string description = null)
			: base(key)
		{
			CategoryKey = categoryKey;
			When = when;
			Amount = amount;
			Description = description;
		}

		[DataMember]
		public string CategoryKey { get; private set; }

		[DataMember]
		public LocalDate When { get; private set; }

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
		public string Description { get; private set; }

		public void UpdateAmount(Money amount)
		{
			bool shouldRaise = Key == 0 || !Amount.Equals(amount);
			Amount = amount;

			if (shouldRaise) Raise(new ExpenseMonthLogChanged(When));
		}

		public void UpdateDescription(string description)
		{
			Description = description;
		}

		public void UpdateCategory(string categoryKey)
		{
			string oldValue = CategoryKey;
			CategoryKey = categoryKey;

			bool shouldRaise = Key != 0 && oldValue != categoryKey;
			if (shouldRaise) Raise(new ExpenseMonthLogChanged(When));
		}
	}
}