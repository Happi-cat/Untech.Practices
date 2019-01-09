using System;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.IncomeLog.MonthLog;
using Untech.Practices;

namespace MyBudgetPlan.Domain.IncomeLog.Forecast
{
	[DataContract]
	public class ProjectedIncome : BudgetLogEntry
	{
		public ProjectedIncome(CreateProjectedIncome request)
			: this(0, request.When, request.Amount, request.Description)
		{
		}

		public ProjectedIncome(int key, YearMonth when, Money amount, string description = null)
			: base(key)
		{
			When = when;
			Amount = amount;
			Description = description;
		}

		[DataMember]
		public YearMonth When { get; private set; }

		public void Update(UpdateProjectedIncome request)
		{
			UpdateAmount(request.Amount);
			UpdateDescription(request.Description);
		}

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