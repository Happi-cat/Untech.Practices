using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyBudgetPlan.Domain;
using Newtonsoft.Json;
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

	public class BudgetLogDataStorage<T> : IAsyncDataStorage<T>,
		IQueryAsyncHandler<BudgetLogQuery<T>, IEnumerable<T>>
		where T : BudgetLogEntry
	{
		private readonly IUserContext _userContext;

		public BudgetLogDataStorage(IUserContext userContext)
		{
			_userContext = userContext;
		}

		public async Task<T> FindAsync(int key, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}

		public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}

		public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}

		public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}

		public async Task<IEnumerable<T>> HandleAsync(BudgetLogQuery<T> request, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}