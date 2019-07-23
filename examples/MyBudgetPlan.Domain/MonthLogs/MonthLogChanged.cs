using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.MonthLogs
{
	public class MonthLogChanged : IEvent
	{
		public MonthLogChanged(BudgetLogType log, YearMonth when)
		{
			Log = log;
			When = when;
		}

		public BudgetLogType Log { get; }
		public YearMonth When { get; }
	}
}