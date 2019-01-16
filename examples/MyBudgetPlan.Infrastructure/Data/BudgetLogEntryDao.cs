using System;
using MyBudgetPlan.Domain;
using Shared.Infrastructure.Data;
using Untech.Practices;

namespace MyBudgetPlan.Infrastructure.Data
{
	public class BudgetLogEntryDao<T> : IUserScopedDao
		where T: BudgetLogEntry
	{
		private BudgetLogEntryDao()
		{

		}

		public BudgetLogEntryDao(int key, int userKey, BudgetLogType log, DateTime when, Money amount)
		{
			Key = key;
			UserKey = userKey;
			Log = log;
			When = when;
			Currency = amount.Currency.Id;
			Amount = amount.Amount;
		}

		public int Key { get; private set; }
		public int UserKey { get; private set; }
		public BudgetLogType Log { get; protected set; }
		public DateTime When { get; private set; }
		public string Currency { get; private set; }
		public double Amount { get; private set; }

		public string Category { get; set; }
		public string Description { get; set; }
	}
}