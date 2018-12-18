using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Forecast
{
	public class DeleteProjectedExpense : ICommand<ProjectedExpense>
	{
		public DeleteProjectedExpense(int key)
		{
			Key = key;
		}

		public int Key { get; }
	}
}