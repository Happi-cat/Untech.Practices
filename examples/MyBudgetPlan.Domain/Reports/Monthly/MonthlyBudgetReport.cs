using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.Categories;
using MyBudgetPlan.Domain.MonthLogs;

namespace MyBudgetPlan.Domain.Reports.Monthly
{
	[DataContract]
	public class MonthlyBudgetReport
	{
		private MonthlyBudgetReport()
		{
		}

		public MonthlyBudgetReport(YearMonth when, IMoneyCalculator calculator,
			IEnumerable<Category> categories,
			MonthLog incomeLog,
			MonthLog expenseLog)
		{
			if (incomeLog.Log != BudgetLogType.Incomes) throw new ArgumentException(nameof(incomeLog));
			if (expenseLog.Log != BudgetLogType.Expenses) throw new ArgumentException(nameof(expenseLog));

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

		private static IEnumerable<ExpenseCategoryReport> GetExpensesReport(IMoneyCalculator calculator,
			IEnumerable<Category> categories,
			MonthLog expenseLog)
		{
			return categories.Select(category => new ExpenseCategoryReport(
				category,
				calculator,
				expenseLog
			));
		}
	}
}