using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.MonthLog
{
	public class ExpenseMonthLogQuery : IQuery<ExpenseMonthLog>
	{
		public ExpenseMonthLogQuery(YearMonth when)
		{
			When = when;
		}

		public YearMonth When { get; }
	}
}