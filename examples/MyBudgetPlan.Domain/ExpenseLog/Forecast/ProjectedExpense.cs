using System;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.ExpenseLog.MonthLog;
using Untech.Practices;

namespace MyBudgetPlan.Domain.ExpenseLog.Forecast
{
	[DataContract]
	public class ProjectedExpense : BudgetLogEntry
	{
		private ProjectedExpense()
		{
		}

		public ProjectedExpense(CreateProjectedExpense request)
			: this(0, request.CategoryKey, request.When, request.Amount, request.Description)
		{
			Raise(new ExpenseMonthLogChanged(request.When));
		}

		public ProjectedExpense(int key, string categoryKey, YearMonth when, Money amount, string description = null)
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
		public YearMonth When { get; private set; }

		public void Update(UpdateProjectedExpense request)
		{
			UpdateCategory(request.CategoryKey);
			UpdateAmount(request.Amount);
			UpdateDescription(request.Description);
		}

		public void UpdateAmount(Money amount)
		{
			if (amount.Amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));

			Amount = amount;
			Raise(new ExpenseMonthLogChanged(When));
		}

		public void UpdateCategory(string categoryKey)
		{
			string oldValue = CategoryKey;
			CategoryKey = categoryKey;

			bool shouldRaise = Key != 0 && oldValue != categoryKey;
			if (shouldRaise) Raise(new ExpenseMonthLogChanged(When));
		}

		public void UpdateDescription(string description)
		{
			Description = description;
		}
	}
}