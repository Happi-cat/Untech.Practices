using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Untech.Practices;

namespace MyBudgetPlan.Domain
{
	[DataContract]
	public class FinancialStats
	{
		private FinancialStats()
		{
		}

		public FinancialStats(Money actual, Money forecast, IMoneyCalculator calculator)
		{
			Forecast = forecast;
			Actual = actual;
			Difference = calculator.Minus(Actual, Forecast);
		}

		public static FinancialStats GetTotal(IMoneyCalculator calculator,
			IEnumerable<Money> actual,
			IEnumerable<Money> forecast)
		{
			return new FinancialStats(
				calculator.Sum(actual),
				calculator.Sum(forecast),
				calculator);
		}

		public static FinancialStats GetTotal(IMoneyCalculator calculator, IEnumerable<FinancialStats> items)
		{
			var list = items.ToList();
			return new FinancialStats(
				calculator.Sum(list.Select(n => n.Actual)),
				calculator.Sum(list.Select(n => n.Forecast)),
				calculator);
		}

		public static FinancialStats GetBalance(IMoneyCalculator calculator,
			FinancialStats income,
			FinancialStats expense)
		{
			return new FinancialStats(
				calculator.Minus(income.Actual, expense.Actual),
				calculator.Minus(income.Forecast, expense.Forecast),
				calculator);
		}

		[DataMember]
		public Money Actual { get; private set; }

		[DataMember]
		public Money Forecast { get; private set; }

		[DataMember]
		public Money Difference { get; private set; }
	}
}