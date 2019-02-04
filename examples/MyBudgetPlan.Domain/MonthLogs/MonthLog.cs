using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.Forecasts;
using MyBudgetPlan.Domain.Transactions;

namespace MyBudgetPlan.Domain.MonthLogs
{
	[DataContract]
	public class MonthLog
	{
		private MonthLog()
		{
		}

		public MonthLog(BudgetLogType log,
			YearMonth when,
			IMoneyCalculator calculator,
			IEnumerable<Transaction> actual = null,
			IEnumerable<Forecast> forecast = null)
		{
			When = when;
			Transactions = actual?.Where(n => n.Log == log).ToList() ?? new List<Transaction>();
			Forecasts = forecast?.Where(n => n.Log == log).ToList() ?? new List<Forecast>();

			Total = FinancialStats.GetTotal(calculator,
				Transactions.Select(n => n.Amount),
				Forecasts.Select(n => n.Amount));
		}

		[DataMember]
		public BudgetLogType Log { get; private set; }

		[DataMember]
		public YearMonth When { get; private set; }

		[DataMember]
		public FinancialStats Total { get; private set; }

		[DataMember]
		public IReadOnlyList<Transaction> Transactions { get; private set; }

		[DataMember]
		public IReadOnlyList<Forecast> Forecasts { get; private set; }
	}
}