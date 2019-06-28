using System;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;

namespace Untech.Practices.DataStorage.Linq2Db
{
	public class GenericDataStorage<T, TKey> : IDataStorage<T, TKey>
		where T : class, IAggregateRoot<TKey>
	{
		private readonly Func<IDataContext> _contextFactory;

		public GenericDataStorage(Func<IDataContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public virtual async Task<T> GetAsync(TKey key,
			CancellationToken cancellationToken = default)
		{
			using (IDataContext context = GetContext())
			{
				return await Table(context).SingleOrDefaultAsync(n => n.Key.Equals(key), cancellationToken)
					?? throw new AggregateRootNotFoundException(key);
			}
		}

		public virtual async Task<T> CreateAsync(T entity,
			CancellationToken cancellationToken = default)
		{
			using (IDataContext context = GetContext())
			{
				await context.InsertWithIdentityAsync(entity, token: cancellationToken);

				return await Table(context).SingleAsync(n => n.Key.Equals(entity.Key), cancellationToken);
			}
		}

		public virtual async Task<bool> DeleteAsync(T entity,
			CancellationToken cancellationToken = default)
		{
			using (IDataContext context = GetContext())
			{
				return await context.DeleteAsync(entity, token: cancellationToken) > 0;
			}
		}

		public virtual async Task<T> UpdateAsync(T entity,
			CancellationToken cancellationToken = default)
		{
			using (IDataContext context = GetContext())
			{
				await context.UpdateAsync(entity, token: cancellationToken);
				return entity;
			}
		}

		protected IDataContext GetContext()
		{
			return _contextFactory();
		}

		protected ITable<T> Table(IDataContext context)
		{
			return context.GetTable<T>();
		}
	}

	public class GenericDataStorage<T, TDao, TKey> : IDataStorage<T, TKey>
		where T : IAggregateRoot<TKey>
		where TDao : class, IAggregateRoot<TKey>
	{
		private readonly Func<IDataContext> _contextFactory;
		private readonly GenericDataStorage<TDao, TKey> _innerDataStorage;
		private readonly IEntityMapper<T, TDao, TKey> _mapper;

		public GenericDataStorage(Func<IDataContext> contextFactory, IEntityMapper<T, TDao, TKey> mapper)
		{
			_innerDataStorage = new GenericDataStorage<TDao, TKey>(contextFactory);
			_contextFactory = contextFactory;
			_mapper = mapper;
		}

		public virtual async Task<T> GetAsync(TKey key,
			CancellationToken cancellationToken = default)
		{
			return ToEntity(await _innerDataStorage.GetAsync(key, cancellationToken));
		}

		public virtual async Task<T> CreateAsync(T entity,
			CancellationToken cancellationToken = default)
		{
			return ToEntity(await _innerDataStorage.CreateAsync(ToDao(entity), cancellationToken));
		}

		public virtual async Task<bool> DeleteAsync(T entity,
			CancellationToken cancellationToken = default)
		{
			return await _innerDataStorage.DeleteAsync(ToDao(entity), cancellationToken);
		}

		public virtual async Task<T> UpdateAsync(T entity,
			CancellationToken cancellationToken = default)
		{
			return ToEntity(await _innerDataStorage.UpdateAsync(ToDao(entity), cancellationToken));
		}

		protected IDataContext GetContext()
		{
			return _contextFactory();
		}

		protected ITable<TDao> GetTable(IDataContext context)
		{
			return context.GetTable<TDao>();
		}

		protected T ToEntity(TDao dao)
		{
			return _mapper.Map(dao);
		}

		protected TDao ToDao(T entity)
		{
			return _mapper.Map(entity);
		}
	}
}