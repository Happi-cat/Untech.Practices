using System;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.IncomeLog.MonthLog;
using NodaTime;
using Untech.Practices;

namespace MyBudgetPlan.Domain.IncomeLog.Actual
{
	[DataContract]
	public class ActualIncome : BudgetLogEntry
	{
		private ActualIncome()
		{
		}

		public ActualIncome(CreateActualIncome request)
			: this(0, request.When, request.Amount, request.Description)
		{
		}

		public ActualIncome(int key, LocalDate when, Money amount, string description = null)
			: base(key)
		{
			When = when;
			Amount = amount;
			Description = description;
		}

		[DataMember]
		public LocalDate When { get; private set; }

		public void Update(UpdateActualIncome request)
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