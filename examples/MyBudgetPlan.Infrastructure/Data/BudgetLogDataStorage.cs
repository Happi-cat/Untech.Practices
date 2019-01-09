using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MyBudgetPlan.Domain;
using Newtonsoft.Json;
using NodaTime;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;
using Untech.Practices.UserContext;

namespace MyBudgetPlan.Infrastructure.Data
{
	public class BudgetLogDao<T>
	{
		public int Key { get; set; }
		public int UserKey { get; set; }
		public DateTime When { get; set; }
		public string Json { get; set; }

		public T GetItem()
		{
			return JsonConvert.DeserializeObject<T>(Json);
		}

		public void SetItem(T item)
		{
			Json = JsonConvert.SerializeObject(item);
		}
	}

	public class BudgetLogDataStorage<T, TTable, TMapper> : IAsyncDataStorage<T>,
		IQueryAsyncHandler<BudgetLogQuery<T>, IEnumerable<T>>
		where T : BudgetLogEntry
		where TTable: BudgetLogDao<T>
		where TMapper: struct, IDaoMapper<T, TTable>
	{
		private readonly IUserContext _userContext;
		private readonly Func<IDataContext> _dataContext;

		public BudgetLogDataStorage(IUserContext userContext, Func<IDataContext> dataContext)
		{
			_userContext = userContext;
			_dataContext = dataContext;
		}

		public async Task<T> FindAsync(int key, CancellationToken cancellationToken)
		{
			using (var dataContext = _dataContext())
			{
				var dao = await GetMyItems(dataContext)
						.FirstOrDefaultAsync(n => n.Key == key, cancellationToken)
					?? throw new AggregateRootNotFoundException(key);

				return FromDao(dao);
			}
		}

		public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
		{
			var dao = ToDao(entity);

			using (var dataContext = _dataContext())
			{
				var key = await dataContext.InsertWithInt32IdentityAsync(dao, token: cancellationToken);

				dao = await GetMyItems(dataContext)
						.FirstAsync(n => n.Key == key, cancellationToken);

				return FromDao(dao);
			}
		}

		public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
		{
			var dao = ToDao(entity);

			using (var dataContext = _dataContext())
			{
				await dataContext.UpdateAsync(dao, token: cancellationToken);

				dao = await GetMyItems(dataContext)
					.FirstAsync(n => n.Key == entity.Key, cancellationToken);

				return FromDao(dao);
			}
		}

		public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken)
		{
			var dao = ToDao(entity);

			using (var dataContext = _dataContext())
			{
				var affected = await dataContext.DeleteAsync(dao, token: cancellationToken);

				return affected > 0;
			}
		}

		public async Task<IEnumerable<T>> HandleAsync(BudgetLogQuery<T> request, CancellationToken cancellationToken)
		{
			var from = ((LocalDate)request.When).ToDateTimeUnspecified();
			var to = from.AddDays(1);

			using (var dataContext = _dataContext())
			{
				var daos = await GetMyItems(dataContext)
					.Where(n => from <= n.When && n.When < to)
					.ToListAsync(cancellationToken);

				return daos.Select(FromDao);
			}
		}

		private TTable ToDao(T entity)
		{
			return default(TMapper).ToDao(_userContext, entity);
		}

		private T FromDao(TTable dao)
		{
			return default(TMapper).FromDao(_userContext, dao);
		}

		private IQueryable<TTable> GetMyItems(IDataContext dataContext)
		{
			return dataContext.GetTable<TTable>()
				.Where(n => n.UserKey == _userContext.UserKey);
		}
	}
}