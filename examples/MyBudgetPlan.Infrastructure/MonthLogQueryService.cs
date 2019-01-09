using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain;
using MyBudgetPlan.Domain.ExpenseLog.Actual;
using MyBudgetPlan.Domain.ExpenseLog.Forecast;
using MyBudgetPlan.Domain.ExpenseLog.MonthLog;
using MyBudgetPlan.Domain.IncomeLog.Actual;
using MyBudgetPlan.Domain.IncomeLog.Forecast;
using MyBudgetPlan.Domain.IncomeLog.MonthLog;
using MyBudgetPlan.Infrastructure.Data;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace MyBudgetPlan.Infrastructure
{
	public class MonthLogQueryService : IQueryAsyncHandler<IncomeMonthLogQuery, IncomeMonthLog>,
		IQueryAsyncHandler<ExpenseMonthLogQuery, ExpenseMonthLog>
	{
		private readonly IMoneyCalculator _moneyCalculator;
		private readonly IQueryDispatcher _queryDispatcher;

		public MonthLogQueryService(IMoneyCalculator moneyCalculator, IQueryDispatcher queryDispatcher)
		{
			_moneyCalculator = moneyCalculator;
			_queryDispatcher = queryDispatcher;
		}

		public async Task<IncomeMonthLog> HandleAsync(IncomeMonthLogQuery request, CancellationToken cancellationToken)
		{
			var actual = await FetchBudgetLogAsync<ActualIncome>(request.When, cancellationToken);
			var forecast = await FetchBudgetLogAsync<ProjectedIncome>(request.When, cancellationToken);

			return new IncomeMonthLog(request.When, _moneyCalculator, actual, forecast);
		}

		public async Task<ExpenseMonthLog> HandleAsync(ExpenseMonthLogQuery request,
			CancellationToken cancellationToken)
		{
			var actual = await FetchBudgetLogAsync<ActualExpense>(request.When, cancellationToken);
			var forecast = await FetchBudgetLogAsync<ProjectedExpense>(request.When, cancellationToken);

			return new ExpenseMonthLog(request.When, _moneyCalculator, actual, forecast);
		}

		private Task<IEnumerable<T>> FetchBudgetLogAsync<T>(YearMonth when, CancellationToken cancellationToken)
			where T: BudgetLogEntry
		{
			return _queryDispatcher.FetchAsync(new BudgetLogQuery<T>(when), cancellationToken);
		}
	}
}