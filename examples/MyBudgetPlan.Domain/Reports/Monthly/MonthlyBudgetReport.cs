using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.ExpenseLog.Category;
using MyBudgetPlan.Domain.ExpenseLog.MonthLog;
using MyBudgetPlan.Domain.IncomeLog.MonthLog;
using Untech.Practices;

namespace MyBudgetPlan.Domain.Reports.Monthly
{
	[DataContract]
	public class MonthlyBudgetReport
	{
		private MonthlyBudgetReport()
		{
		}

		public MonthlyBudgetReport(YearMonth when, IMoneyCalculator calculator,
			IEnumerable<ExpenseCategory> categories,
			IncomeMonthLog incomeLog,
			ExpenseMonthLog expenseLog)
		{
			When = when;

			Income = incomeLog.Total;
			Expense = expenseLog.Total;
			Balance = FinancialStats.GetBalance(calculator, Income, Expense);

			Expenses = GetExpensesReport(calculator, categories, expenseLog);
		}

		[DataMember]
		public YearMonth When { get; private set; }

		[DataMember]
		public FinancialStats Income { get; private set; }

		[DataMember]
		public FinancialStats Expense { get; private set; }

		[DataMember]
		public FinancialStats Balance { get; private set; }

		[DataMember]
		public IEnumerable<ExpenseCategoryReport> Expenses { get; private set; }

		private IEnumerable<ExpenseCategoryReport> GetExpensesReport(IMoneyCalculator calculator,
			IEnumerable<ExpenseCategory> categories,
			ExpenseMonthLog expenseLog)
		{
			return categories.Select(category => new ExpenseCategoryReport(
				category,
				calculator,
				expenseLog
			));
		}
	}
}