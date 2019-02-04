using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.Reports.Annual
{
	[DataContract]
	public class AnnualBudgetReportQuery : IQuery<AnnualBudgetReport>
	{
	}
}