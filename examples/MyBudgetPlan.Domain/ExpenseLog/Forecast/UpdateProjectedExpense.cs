using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Forecast
{
	public class UpdateProjectedExpense : ICommand<ProjectedExpense>
	{
		public UpdateProjectedExpense(int key, string categoryKey, Money amount, string description = null)
		{
			Key = key;
			CategoryKey = categoryKey;
			Amount = amount;
			Description = description;
		}

		public int Key { get; }
		public string CategoryKey { get; }
		public Money Amount { get; }
		public string Description { get; }
	}
}