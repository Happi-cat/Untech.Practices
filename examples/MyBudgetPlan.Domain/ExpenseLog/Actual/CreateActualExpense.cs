using NodaTime;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Actual
{
	public class CreateActualExpense : ICommand<ActualExpense>
	{
		public CreateActualExpense(string categoryKey, LocalDate when, Money amount, string description = null)
		{
			CategoryKey = categoryKey;
			When = when;
			Amount = amount;
			Description = description;
		}

		public string CategoryKey { get; }

		public LocalDate When { get; }

		public Money Amount { get; }

		public string Description { get; }
	}
}