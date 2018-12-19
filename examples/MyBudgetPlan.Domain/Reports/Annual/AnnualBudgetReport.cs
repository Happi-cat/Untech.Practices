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

			ProjectedIncome = calculator.Sum(Months.Select(n => n.ProjectedIncome));
			ProjectedExpense = calculator.Sum(Months.Select(n => n.ProjectedExpense));
			ProjectedBalance = calculator.Sum(Months.Select(n => n.ProjectedBalance));

			ActualIncome = calculator.Sum(Months.Select(n => n.ActualIncome));
			ActualExpense = calculator.Sum(Months.Select(n => n.ActualExpense));
			ActualBalance = calculator.Sum(Months.Select(n => n.ActualBalance));
		}

		[DataMember]
		public Money ProjectedIncome { get; private set; }

		[DataMember]
		public Money ProjectedExpense { get; private set; }

		[DataMember]
		public Money ProjectedBalance { get; private set; }

		[DataMember]
		public Money ActualIncome { get; private set; }

		[DataMember]
		public Money ActualExpense { get; private set; }

		[DataMember]
		public Money ActualBalance { get; private set; }

		[DataMember]
		public IReadOnlyList<MonthlyBudgetReport> Months { get; private set; }
	}
}