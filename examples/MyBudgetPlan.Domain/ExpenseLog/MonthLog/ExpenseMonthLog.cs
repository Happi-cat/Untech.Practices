using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.ExpenseLog.Actual;
using MyBudgetPlan.Domain.ExpenseLog.Forecast;
using Untech.Practices;

namespace MyBudgetPlan.Domain.ExpenseLog.MonthLog
{
	[DataContract]
	public class ExpenseMonthLog
	{
		private ExpenseMonthLog()
		{
		}

		public ExpenseMonthLog(YearMonth when,
			IMoneyCalculator calculator,
			IEnumerable<ActualExpense> actual = null,
			IEnumerable<ProjectedExpense> forecast = null)
		{
			When = when;
			Transactions = actual?.ToList() ?? new List<ActualExpense>();
			Forecast = forecast?.ToList() ?? new List<ProjectedExpense>();

			Total = FinancialStats.GetTotal(calculator,
				Transactions.Select(n => n.Amount),
				Forecast.Select(n => n.Amount));
		}

		[DataMember]
		public YearMonth When { get; private set; }

		[DataMember]
		public FinancialStats Total { get; private set; }

		[DataMember]
		public IReadOnlyList<ActualExpense> Transactions { get; private set; }

		[DataMember]
		public IReadOnlyList<ProjectedExpense> Forecast { get; private set; }
	}
}