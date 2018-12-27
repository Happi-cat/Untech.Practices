using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.MonthLog
{
	[DataContract]
	public class ExpenseMonthLogQuery : IQuery<ExpenseMonthLog>
	{
		private ExpenseMonthLogQuery()
		{

		}

		public ExpenseMonthLogQuery(YearMonth when)
		{
			When = when;
		}

		[DataMember]
		public YearMonth When { get; private set; }
	}
}