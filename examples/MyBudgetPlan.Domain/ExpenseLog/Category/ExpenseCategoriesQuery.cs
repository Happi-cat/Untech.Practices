using System.Collections.Generic;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Category
{
	public class ExpenseCategoriesQuery : IQuery<IEnumerable<ExpenseCategory>>
	{

	}
}