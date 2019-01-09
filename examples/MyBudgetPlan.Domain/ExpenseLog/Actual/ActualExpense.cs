using System.Runtime.Serialization;
using MyBudgetPlan.Domain.ExpenseLog.MonthLog;
using NodaTime;
using Untech.Practices;

namespace MyBudgetPlan.Domain.ExpenseLog.Actual
{
	[DataContract]
	public class ActualExpense : BudgetLogEntry
	{
		private ActualExpense()
		{
		}

		public ActualExpense(CreateActualExpense request)
			: this(0, request.CategoryKey, request.When, request.Amount, request.Description)
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

		public void Update(UpdateActualExpense request)
		{
			UpdateCategory(request.CategoryKey);
			UpdateAmount(request.Amount);
			UpdateDescription(request.Description);
		}

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