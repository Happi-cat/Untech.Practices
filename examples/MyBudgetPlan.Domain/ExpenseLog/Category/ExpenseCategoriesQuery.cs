using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Category
{
	[DataContract]
	public class ExpenseCategoriesQuery : IQuery<IEnumerable<ExpenseCategory>>
	{
	}
}