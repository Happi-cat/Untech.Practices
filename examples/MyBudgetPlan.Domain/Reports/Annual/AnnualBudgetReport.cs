using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.Reports.Monthly;
using Untech.Practices;

namespace MyBudgetPlan.Domain.Reports.Annual
{
	[DataContract]
	public class AnnualBudgetReport
	{
		private AnnualBudgetReport()
		{
		}

		public AnnualBudgetReport(IMoneyCalculator calculator, IEnumerable<MonthlyBudgetReport> months)
		{
			Months = months.ToList();

			Income = FinancialStats.GetTotal(calculator, Months.Select(n => n.Income));
			Expense = FinancialStats.GetTotal(calculator, Months.Select(n => n.Expense));

			Balance = FinancialStats.GetBalance(calculator, Income, Expense);
		}

		[DataMember]
		public FinancialStats Income { get; private set; }

		[DataMember]
		public FinancialStats Expense { get; private set; }

		[DataMember]
		public FinancialStats Balance { get; private set; }

		[DataMember]
		public IReadOnlyList<MonthlyBudgetReport> Months { get; private set; }
	}
}