using System;
using MyBudgetPlan.Domain;
using Shared.Infrastructure.Data;

namespace MyBudgetPlan.Infrastructure.Data
{
	public abstract class BudgetLogEntryDao<T> : IUserScopedDao
		where T: BudgetLogEntry
	{
		public int Key { get; set; }
		public int UserKey { get; set; }
		public DateTime When { get; set; }
	}
}