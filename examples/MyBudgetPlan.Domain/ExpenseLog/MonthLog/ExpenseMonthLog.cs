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
			Actual = actual?.ToList() ?? new List<ActualExpense>();
			Forecast = forecast?.ToList() ?? new List<ProjectedExpense>();

			ActualTotal = CalcActualTotal(calculator);
			ProjectedTotal = CalcProjectedTotal(calculator);
			Difference = calculator.Minus(ActualTotal, ProjectedTotal);
		}

		[DataMember]
		public YearMonth When { get; private set; }

		[DataMember]
		public Money ActualTotal { get; private set; }

		[DataMember]
		public Money ProjectedTotal { get; private set; }

		[DataMember]
		public Money Difference { get; private set; }


		[DataMember]
		public IReadOnlyList<ActualExpense> Actual { get; private set; }

		[DataMember]
		public IReadOnlyList<ProjectedExpense> Forecast { get; private set; }

		private Money CalcActualTotal(IMoneyCalculator calculator)
		{
			return calculator.Sum(Actual.Select(n => n.Amount));
		}

		private Money CalcProjectedTotal(IMoneyCalculator calculator)
		{
			return calculator.Sum(Forecast.Select(n => n.Amount));
		}
	}
}