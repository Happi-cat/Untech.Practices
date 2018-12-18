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

	public class UpdateProjectedIncome : ICommand<ProjectedIncome>
	{
		public UpdateProjectedIncome(int key, string categoryKey, Money amount, string description = null)
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

	public class DeleteProjectedIncome : ICommand<ProjectedIncome>
	{
		public DeleteProjectedIncome(int key)
		{
			Key = key;
		}

		public int Key { get; }
	}
}