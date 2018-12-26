using System.Linq;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.ExpenseLog.Category;
using MyBudgetPlan.Domain.ExpenseLog.MonthLog;
using Untech.Practices;

namespace MyBudgetPlan.Domain.Reports.Monthly
{
	[DataContract]
	public class ExpenseCategoryReport
	{
		public ExpenseCategoryReport(ExpenseCategory category,
			IMoneyCalculator calculator,
			ExpenseMonthLog expenseLog)
		{
			CategoryKey = category.Key;
			Title = category.Title;
			Description = category.Description;

			Total = FinancialStats.GetTotal(calculator,
				expenseLog.Transactions
					.Where(n => category.IsSameOrParentOf(n.CategoryKey))
					.Select(n => n.Amount),
				expenseLog.Forecast
					.Where(n => category.IsSameOrParentOf(n.CategoryKey))
					.Select(n => n.Amount)
			);
		}

		[DataMember]
		public string CategoryKey { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public string Description { get; private set; }

		[DataMember]
		public FinancialStats Total { get; private set; }
	}
}