using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.IncomeLog.Actual;
using MyBudgetPlan.Domain.IncomeLog.Forecast;
using Untech.Practices;

namespace MyBudgetPlan.Domain.IncomeLog.MonthLog
{
	[DataContract]
	public class IncomeMonthLog
	{
		private IncomeMonthLog()
		{
		}

		public IncomeMonthLog(YearMonth when,
			IMoneyCalculator calculator,
			IEnumerable<ActualIncome> actual = null,
			IEnumerable<ProjectedIncome> forecast = null)
		{
			When = when;
			Transactions = actual?.ToList() ?? new List<ActualIncome>();
			Forecast = forecast?.ToList() ?? new List<ProjectedIncome>();

			Total = FinancialStats.GetTotal(calculator,
				Transactions.Select(n => n.Amount),
				Forecast.Select(n => n.Amount));
		}

		[DataMember]
		public YearMonth When { get; private set; }

		[DataMember]
		public FinancialStats Total { get; private set; }

		[DataMember]
		public IReadOnlyList<ActualIncome> Transactions { get; private set; }

		[DataMember]
		public IReadOnlyList<ProjectedIncome> Forecast { get; private set; }
	}
}