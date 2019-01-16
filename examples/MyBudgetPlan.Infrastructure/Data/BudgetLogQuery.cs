using System.Collections.Generic;
using MyBudgetPlan.Domain;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Infrastructure.Data
{
	public class BudgetLogQuery<T> : IQuery<IEnumerable<T>>
		where T: BudgetLogEntry
	{
		public BudgetLogQuery(BudgetLogType log, YearMonth when)
		{
			Log = log;
			When = when;
		}

		public BudgetLogType Log { get; private set; }
		public YearMonth When { get; private set; }
	}
}