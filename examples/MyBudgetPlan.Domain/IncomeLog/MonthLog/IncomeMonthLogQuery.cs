using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.MonthLog
{
	[DataContract]
	public class IncomeMonthLogQuery : IQuery<IncomeMonthLog>
	{
		private IncomeMonthLogQuery()
		{

		}

		public IncomeMonthLogQuery(YearMonth when)
		{
			When = when;
		}

		[DataMember]
		public YearMonth When { get; private set; }
	}
}