using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.Actual
{
	public class DeleteActualIncome : ICommand<ActualIncome>
	{
		public DeleteActualIncome(int key)
		{
			Key = key;
		}

		public int Key { get; }
	}
}