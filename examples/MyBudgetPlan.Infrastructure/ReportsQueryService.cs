using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain;
using MyBudgetPlan.Domain.Categories;
using MyBudgetPlan.Domain.MonthLogs;
using MyBudgetPlan.Domain.Reports.Annual;
using MyBudgetPlan.Domain.Reports.Monthly;
using NodaTime;
using Untech.Practices.CQRS;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.Persistence.Cache;
using Untech.Practices.UserContext;

namespace MyBudgetPlan.Infrastructure
{
	public class ReportsQueryService : IQueryHandler<MonthlyBudgetReportQuery, MonthlyBudgetReport>,
		IQueryHandler<AnnualBudgetReportQuery, AnnualBudgetReport>,
		IEventHandler<MonthLogChanged>
	{
		private readonly IUserContext _userContext;
		private readonly IMoneyCalculator _moneyCalculator;
		private readonly IQueryDispatcher _queryDispatcher;
		private readonly ICacheStorage _cacheStorage;

		public ReportsQueryService(IUserContext userContext,
			IMoneyCalculator moneyCalculator,
			IQueryDispatcher queryDispatcher,
			ICacheStorage cacheStorage)
		{
			_userContext = userContext;
			_moneyCalculator = moneyCalculator;
			_queryDispatcher = queryDispatcher;
			_cacheStorage = cacheStorage;
		}

		public async Task<MonthlyBudgetReport> HandleAsync(MonthlyBudgetReportQuery request, CancellationToken cancellationToken)
		{
			var reportCacheKey = GetCacheKey(request.When);
			var cachedReport = await _cacheStorage.GetAsync<MonthlyBudgetReport>(reportCacheKey, cancellationToken);

			if (cachedReport.HasValue)
			{
				return cachedReport;
			}

			var categories = await FetchAsync(new CategoriesQuery(), cancellationToken);
			var expenseLog = await FetchAsync(new MonthLogQuery(BudgetLogType.Expenses, request.When), cancellationToken);
			var incomeLog = await FetchAsync(new MonthLogQuery(BudgetLogType.Incomes, request.When), cancellationToken);

			var report = new MonthlyBudgetReport(request.When, _moneyCalculator, categories, incomeLog, expenseLog);

			await _cacheStorage.SetAsync(reportCacheKey, report, cancellationToken);

			return report;
		}

		public async Task<AnnualBudgetReport> HandleAsync(AnnualBudgetReportQuery request, CancellationToken cancellationToken)
		{
			var monthlyReports = await GetMonthlyBudgetReportsAsync(cancellationToken);

			return new AnnualBudgetReport(_moneyCalculator, monthlyReports);
		}

		public async Task PublishAsync(MonthLogChanged notification, CancellationToken cancellationToken)
		{
			var cacheKey = GetCacheKey(notification.When);
			await _cacheStorage.DropAsync(cacheKey, cancellationToken);
		}

		private string GetCacheKey(YearMonth when)
		{
			return $"my-budget-plan:{_userContext.UserKey}:monthly-budget-report:{when}";
		}

		private Task<MonthlyBudgetReport[]> GetMonthlyBudgetReportsAsync(CancellationToken cancellationToken)
		{
			var now = LocalDate.FromDateTime(DateTime.UtcNow);

			return Task.WhenAll(Enumerable.Range(-8, 12)
				.Select(shift => new MonthlyBudgetReportQuery(now.PlusMonths(shift)))
				.Select(query => FetchAsync(query, cancellationToken)));
		}

		private Task<T> FetchAsync<T>(IQuery<T> query, CancellationToken cancellationToken)
		{
			return _queryDispatcher.FetchAsync(query, cancellationToken);
		}
	}
}
