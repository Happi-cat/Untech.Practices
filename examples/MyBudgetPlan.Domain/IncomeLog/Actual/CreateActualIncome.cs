using NodaTime;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.Actual
{
	public class CreateActualIncome : ICommand<ActualIncome>
	{
		public CreateActualIncome(string categoryKey, LocalDate when, Money amount, string description = null)
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

	public class UpdateActualIncome : ICommand<ActualIncome>
	{
		public int Key { get; }
		public string CategoryKey { get; }
		public Money Amount { get; }
		public string Description { get; }

		public UpdateActualIncome(int key, string categoryKey, Money amount, string description = null)
		{
			Key = key;
			CategoryKey = categoryKey;
			Amount = amount;
			Description = description;
		}
	}

	public class DeleteActualIncome : ICommand<ActualIncome>
	{
		public DeleteActualIncome(int key)
		{
			Key = key;
		}

		public int Key { get; }

	}
}