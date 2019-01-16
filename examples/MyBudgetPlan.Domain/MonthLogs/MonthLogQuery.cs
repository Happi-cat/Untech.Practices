using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.MonthLogs
{
	[DataContract]
	public class MonthLogQuery : IQuery<MonthLog>
	{
		private MonthLogQuery()
		{

		}

		public MonthLogQuery(BudgetLogType log, YearMonth when)
		{
			Log = log;
			When = when;
		}

		[DataMember]
		public BudgetLogType Log { get; private set; }

		[DataMember]
		public YearMonth When { get; private set; }
	}
}