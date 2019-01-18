using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MyBudgetPlan.Domain;
using NodaTime;
using Shared.Infrastructure.Data;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.UserContext;

namespace MyBudgetPlan.Infrastructure.Data
{
	public class BudgetLogDataStorage<T, TTable, TMapper> : UserScopedGenericDataStorage<T, TTable, TMapper>,
		IQueryAsyncHandler<BudgetLogQuery<T>, IEnumerable<T>>
		where T : BudgetLogEntry
		where TTable : BudgetLogEntryDao<T>
		where TMapper : struct, IDaoMapper<T, TTable>
	{
		public BudgetLogDataStorage(IUserContext userContext, Func<IDataContext> dataContext) : base(userContext, dataContext)
		{
		}

		public async Task<IEnumerable<T>> HandleAsync(BudgetLogQuery<T> request, CancellationToken cancellationToken)
		{
			var from = ((LocalDate)request.When).ToDateTimeUnspecified();
			var to = from.AddMonths(1);

			using (var dataContext = GetContext())
			{
				var daos = await GetMyItems(dataContext)
					.Where(n => n.Log == request.Log)
					.Where(n => from <= n.When && n.When < to)
					.ToListAsync(cancellationToken);

				return daos.Select(FromDao);
			}
		}
	}
}