using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LinqToDB;
using LinqToDB.Mapping;

namespace Untech.Practices.DataStorage.Linq2Db
{
	[PublicAPI]
	public class GenericDataStorage<T> : GenericDataStorage<T, int>, IDataStorage<T>
		where T : class, IHasKey
	{
		public GenericDataStorage(Func<IDataContext> contextFactory) : base(contextFactory)
		{
		}
	}

	[PublicAPI]
	public class GenericDataStorage<T, TKey> : IDataStorage<T, TKey>
		where T : class, IHasKey<TKey>
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
					?? throw new ItemNotFoundException(key);
			}
		}

		public virtual async Task<T> CreateAsync(T entity,
			CancellationToken cancellationToken = default)
		{
			using (IDataContext context = GetContext())
			{
				var key = TableHasIdentity(context) ? await InsertWithIdentity(context) : await Insert(context);

				return await Table(context).SingleAsync(n => n.Key.Equals(key), cancellationToken);
			}

			bool TableHasIdentity(IDataContext context)
			{
				return EntityDescriptor(context).Columns.Any(c => c.IsIdentity);
			}

			async Task<TKey> InsertWithIdentity(IDataContext context)
			{
				var identityObj = await context.InsertWithIdentityAsync(entity, token: cancellationToken);
				return context.MappingSchema.ChangeTypeTo<TKey>(identityObj);
			}

			async Task<TKey> Insert(IDataContext context)
			{
				await context.InsertAsync(entity, token: cancellationToken);
				return entity.Key;
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

		protected EntityDescriptor EntityDescriptor(IDataContext context)
		{
			return context.MappingSchema.GetEntityDescriptor(typeof(T));
		}
	}

	[PublicAPI]
	public class GenericDataStorage<TEntity, TData, TKey> : IDataStorage<TEntity, TKey>
		where TEntity : IHasKey<TKey>
		where TData : class, IHasKey<TKey>
	{
		private readonly Func<IDataContext> _contextFactory;
		private readonly GenericDataStorage<TData, TKey> _innerDataStorage;
		private readonly IEntityMapper<TEntity, TData, TKey> _mapper;

		public GenericDataStorage(Func<IDataContext> contextFactory, IEntityMapper<TEntity, TData, TKey> mapper)
		{
			_innerDataStorage = new GenericDataStorage<TData, TKey>(contextFactory);
			_contextFactory = contextFactory;
			_mapper = mapper;
		}

		public virtual async Task<TEntity> GetAsync(TKey key,
			CancellationToken cancellationToken = default)
		{
			return ToEntity(await _innerDataStorage.GetAsync(key, cancellationToken));
		}

		public virtual async Task<TEntity> CreateAsync(TEntity entity,
			CancellationToken cancellationToken = default)
		{
			return ToEntity(await _innerDataStorage.CreateAsync(ToData(entity), cancellationToken));
		}

		public virtual async Task<bool> DeleteAsync(TEntity entity,
			CancellationToken cancellationToken = default)
		{
			return await _innerDataStorage.DeleteAsync(ToData(entity), cancellationToken);
		}

		public virtual async Task<TEntity> UpdateAsync(TEntity entity,
			CancellationToken cancellationToken = default)
		{
			return ToEntity(await _innerDataStorage.UpdateAsync(ToData(entity), cancellationToken));
		}

		protected IDataContext GetContext()
		{
			return _contextFactory();
		}

		protected ITable<TData> GetTable(IDataContext context)
		{
			return context.GetTable<TData>();
		}

		protected TEntity ToEntity(TData dao)
		{
			return _mapper.Map(dao);
		}

		protected TData ToData(TEntity entity)
		{
			return _mapper.Map(entity);
		}
	}
}