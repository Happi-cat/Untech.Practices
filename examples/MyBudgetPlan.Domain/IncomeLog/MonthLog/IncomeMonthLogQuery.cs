using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.MonthLog
{
	public class IncomeMonthLogQuery : IQuery<IncomeMonthLog>
	{
		public IncomeMonthLogQuery(YearMonth when)
		{
			When = when;
		}

		public YearMonth When { get; }
	}
}