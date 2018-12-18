using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.MonthLog
{
	public class IncomeMonthLogChanged : INotification
	{
		public IncomeMonthLogChanged(YearMonth when)
		{
			When = when;
		}

		public YearMonth When { get; }
	}
}