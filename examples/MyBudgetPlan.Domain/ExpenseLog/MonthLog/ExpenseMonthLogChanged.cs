using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.MonthLog
{
	public class ExpenseMonthLogChanged : INotification
	{
		public ExpenseMonthLogChanged(YearMonth when)
		{
			When = when;
		}

		public YearMonth When { get; }
	}
}