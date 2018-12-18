using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.Reports.Monthly;
using Untech.Practices;

namespace MyBudgetPlan.Domain.Reports.Year
{
	[DataContract]
	public class YearBudgetReport
	{
		private YearBudgetReport()
		{
		}

		public YearBudgetReport(IMoneyCalculator calculator, IEnumerable<MonthlyBudgetReport> months)
		{
			Months = months.ToList();

			ProjectedBalance = calculator.Sum(Months.Select(n => n.ProjectedBalance));

			ActualBalance = calculator.Sum(Months.Select(n => n.ActualBalance));

			Difference = calculator.Minus(ActualBalance, ProjectedBalance);
		}

		[DataMember]
		public Money ProjectedBalance { get; private set; }

		[DataMember]
		public Money ActualBalance { get; private set; }

		[DataMember]
		public Money Difference { get; private set; }

		[DataMember]
		public IReadOnlyList<MonthlyBudgetReport> Months { get; private set; }
	}
}