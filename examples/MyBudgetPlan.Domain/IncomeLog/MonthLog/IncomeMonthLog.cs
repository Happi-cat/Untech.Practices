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
			Actual = actual?.ToList() ?? new List<ActualIncome>();
			Forecast = forecast?.ToList() ?? new List<ProjectedIncome>();

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
		public IReadOnlyList<ActualIncome> Actual { get; private set; }

		[DataMember]
		public IReadOnlyList<ProjectedIncome> Forecast { get; private set; }

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