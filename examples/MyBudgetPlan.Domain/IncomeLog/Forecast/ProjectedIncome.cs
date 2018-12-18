using System;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.IncomeLog.MonthLog;
using Untech.Practices;

namespace MyBudgetPlan.Domain.IncomeLog.Forecast
{
	public class ProjectedIncome : AggregateRoot
	{
		private Money _amount;

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

		public string Description { get; set; }

		public void UpdateAmount(Money amount)
		{
			if (amount.Amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));

			Amount = amount;
			Raise(new IncomeMonthLogChanged(When));
		}

		public void UpdateDescription(string description)
		{
			Description = description;
		}
	}
}