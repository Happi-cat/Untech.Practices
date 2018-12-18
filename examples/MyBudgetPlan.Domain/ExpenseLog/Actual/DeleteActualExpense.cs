using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Actual
{
	public class DeleteActualExpense : ICommand<ActualExpense>
	{
		public DeleteActualExpense(int key)
		{
			Key = key;
		}

		public int Key { get; }
	}
}