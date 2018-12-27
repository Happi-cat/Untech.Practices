using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.Reports.Monthly
{
	[DataContract]
	public class MonthlyBudgetReportQuery : IQuery<MonthlyBudgetReport>
	{
		private MonthlyBudgetReportQuery()
		{

		}

		public MonthlyBudgetReportQuery(YearMonth when)
		{
			When = when;
		}

		[DataMember]
		public YearMonth When { get; private set; }
	}
}