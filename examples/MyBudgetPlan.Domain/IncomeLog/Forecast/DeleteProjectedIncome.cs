using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.Forecast
{
	public class DeleteProjectedIncome : ICommand<ProjectedIncome>
	{
		public DeleteProjectedIncome(int key)
		{
			Key = key;
		}

		public int Key { get; }
	}
}