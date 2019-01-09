using System;
using Shared.Infrastructure.Data;

namespace MyBudgetPlan.Infrastructure.Data
{
	public abstract class BudgetLogEntryDao<T> : IUserScopedDao
	{
		public DateTime When { get; set; }
		public int Key { get; set; }
		public int UserKey { get; set; }
	}
}