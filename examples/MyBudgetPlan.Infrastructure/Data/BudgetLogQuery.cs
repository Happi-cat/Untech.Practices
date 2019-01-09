using System.Collections.Generic;
using MyBudgetPlan.Domain;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Infrastructure.Data
{
	public class BudgetLogQuery<T> : IQuery<IEnumerable<T>>
		where T: BudgetLogEntry
	{
		public BudgetLogQuery(YearMonth when)
		{
			When = when;
		}

		public YearMonth When { get; private set; }
	}
}