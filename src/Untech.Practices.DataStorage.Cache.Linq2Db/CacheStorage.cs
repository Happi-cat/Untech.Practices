using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;

namespace Untech.Practices.DataStorage.Cache.Linq2Db
{
	public class CacheStorage : ICacheStorage
	{
		private readonly Func<IDataContext> _contextFactory;

		public CacheStorage(Func<IDataContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task DropAsync(string key, CancellationToken cancellationToken = default)
		{
			using (IDataContext context = _contextFactory())
			{
				await Find(context, key)
					.DeleteAsync(cancellationToken);
			}
		}

		public async Task<CacheValue<T>> GetAsync<T>(string key,
			CancellationToken cancellationToken = default)
		{
			using (IDataContext context = _contextFactory())
			{
				CacheEntry entry = await Find(context, key).SingleOrDefaultAsync(cancellationToken);

				return entry == null
					? new CacheValue<T>()
					: new CacheValue<T>(entry.GetValue<T>());
			}
		}

		public async Task SetAsync(string key, object value,
			CancellationToken cancellationToken = default)
		{
			using (IDataContext context = _contextFactory())
			{
				await Find(context, key).DeleteAsync(cancellationToken);

				if (value == null) return;

				CacheEntry entity = new CacheEntry(key, value);

				await context.InsertAsync(entity, token: cancellationToken);
			}
		}

		private IQueryable<CacheEntry> Find(IDataContext context, string key)
		{
			return context
				.GetTable<CacheEntry>()
				.Where(n => n.Key == key);
		}
	}
}