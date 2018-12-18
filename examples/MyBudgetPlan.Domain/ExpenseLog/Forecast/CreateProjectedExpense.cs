using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Forecast
{
	public class CreateProjectedExpense : ICommand<ProjectedExpense>
	{
		public CreateProjectedExpense(string categoryKey, YearMonth when, Money amount, string description = null)
		{
			CategoryKey = categoryKey;
			When = when;
			Amount = amount;
			Description = description;
		}

		public string CategoryKey { get; }

		public YearMonth When { get; }

		public Money Amount { get; }

		public string Description { get; }
	}
}