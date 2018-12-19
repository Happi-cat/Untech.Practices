using NodaTime;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.Forecast
{
	public class CreateProjectedIncome : ICommand<ProjectedIncome>
	{
		public CreateProjectedIncome(string categoryKey, YearMonth when, Money amount, string description = null)
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