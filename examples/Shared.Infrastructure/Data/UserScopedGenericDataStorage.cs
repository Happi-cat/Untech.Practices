using System;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using Untech.Practices.DataStorage;
using Untech.Practices.UserContext;

namespace Shared.Infrastructure.Data
{
	public class UserScopedGenericDataStorage<T, TTable, TMapper> : IDataStorage<T>
		where T : IHasKey
		where TTable : class, IUserScopedDao
		where TMapper : struct, IDaoMapper<T, TTable>
	{
		private readonly IUserContext _userContext;
		private readonly Func<IDataContext> _dataContext;

		public UserScopedGenericDataStorage(IUserContext userContext, Func<IDataContext> dataContext)
		{
			_userContext = userContext;
			_dataContext = dataContext;
		}

		public virtual async Task<T> GetAsync(int key, CancellationToken cancellationToken)
		{
			using (var dataContext = _dataContext())
			{
				var dao = await GetMyItems(dataContext)
						.FirstOrDefaultAsync(n => n.Key == key, cancellationToken)
					?? throw new ItemNotFoundException(key);

				return FromDao(dao);
			}
		}

		public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
		{
			var dao = ToDao(entity);

			EnsureIsMyOrThrow(dao);

			using (var dataContext = _dataContext())
			{
				var key = await dataContext.InsertWithInt32IdentityAsync(dao, token: cancellationToken);

				dao = await GetMyItemAsync(dataContext, key, cancellationToken);

				return FromDao(dao);
			}
		}

		public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
		{
			var dao = ToDao(entity);

			EnsureIsMyOrThrow(dao);

			using (var dataContext = _dataContext())
			{
				await dataContext.UpdateAsync(dao, token: cancellationToken);

				dao = await GetMyItemAsync(dataContext, entity.Key, cancellationToken);

				return FromDao(dao);
			}
		}

		public virtual async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken)
		{
			var dao = ToDao(entity);

			EnsureIsMyOrThrow(dao);

			using (var dataContext = _dataContext())
			{
				var affected = await dataContext.DeleteAsync(dao, token: cancellationToken);

				return affected > 0;
			}
		}

		protected IDataContext GetContext()
		{
			return _dataContext();
		}

		protected TTable ToDao(T entity)
		{
			return default(TMapper).ToDao(_userContext, entity);
		}

		protected T FromDao(TTable dao)
		{
			return default(TMapper).FromDao(_userContext, dao);
		}

		protected IQueryable<TTable> GetMyItems(IDataContext dataContext)
		{
			return dataContext.GetTable<TTable>().Where(n => n.UserKey == _userContext.UserKey);
		}

		protected Task<TTable> GetMyItemAsync(IDataContext dataContext, int key, CancellationToken token)
		{
			return GetMyItems(dataContext).FirstAsync(n => n.Key == key, token);
		}

		protected void EnsureIsMyOrThrow(TTable dao)
		{
			if (dao.UserKey == _userContext.UserKey) return;

			throw new SecurityException("Trying to access wrong item");
		}
	}
}