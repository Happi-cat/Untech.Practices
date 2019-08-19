using MyBudgetPlan.Domain;
using Shared.Infrastructure.Data;

namespace MyBudgetPlan.Infrastructure.Data
{
	public interface IBudgetLogDaoMapper<T> : IDaoMapper<T, BudgetLogEntryDao<T>>
		where T : BudgetLogEntry
	{ }
}