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

			Projected = calculator.Sum(expenseLog.Actual
				.Where(n => category.IsSameOrParentOf(n.CategoryKey))
				.Select(n => n.Amount));

			Actual = calculator.Sum(expenseLog.Forecast
				.Where(n => category.IsSameOrParentOf(n.CategoryKey))
				.Select(n => n.Amount));

			Difference = calculator.Minus(Actual, Projected);
		}

		[DataMember]
		public string CategoryKey { get; private set; }

		[DataMember]
		public string Title { get; private set; }

		[DataMember]
		public string Description { get; private set; }

		[DataMember]
		public Money Projected { get; private set; }

		[DataMember]
		public Money Actual { get; private set; }

		[DataMember]
		public Money Difference { get; private set; }
	}
}