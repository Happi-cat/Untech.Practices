using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain;
using MyBudgetPlan.Domain.Forecasts;
using MyBudgetPlan.Domain.MonthLogs;
using MyBudgetPlan.Domain.Transactions;
using MyBudgetPlan.Infrastructure.Data;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace MyBudgetPlan.Infrastructure
{
	public class MonthLogQueryService : IQueryHandler<MonthLogQuery, MonthLog>
	{
		private readonly IMoneyCalculator _moneyCalculator;
		private readonly IQueryDispatcher _queryDispatcher;

		public MonthLogQueryService(IMoneyCalculator moneyCalculator, IQueryDispatcher queryDispatcher)
		{
			_moneyCalculator = moneyCalculator;
			_queryDispatcher = queryDispatcher;
		}

		public async Task<MonthLog> HandleAsync(MonthLogQuery request, CancellationToken cancellationToken)
		{
			var actual = await FetchBudgetLogAsync<Transaction>(request, cancellationToken);
			var forecast = await FetchBudgetLogAsync<Forecast>(request, cancellationToken);

			return new MonthLog(request.Log, request.When, _moneyCalculator, actual, forecast);
		}

		private Task<IEnumerable<T>> FetchBudgetLogAsync<T>(MonthLogQuery request, CancellationToken cancellationToken)
			where T : BudgetLogEntry
		{
			return _queryDispatcher.FetchAsync(new BudgetLogQuery<T>(request.Log, request.When), cancellationToken);
		}
	}
}