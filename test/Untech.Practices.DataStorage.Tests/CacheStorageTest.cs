using System;
using System.IO;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using Untech.Practices.DataStorage.Cache;
using Untech.Practices.DataStorage.Cache.Linq2Db;
using Xunit;

namespace Untech.Practices.DataStorage
{
	public class CacheStorageTest : IDisposable
	{
		private readonly string _dbFileName;
		private readonly ICacheStorage _cacheStorage;

		public CacheStorageTest()
		{
			_dbFileName = $"{nameof(CacheStorageTest)}.temp.db";
			_cacheStorage = new CacheStorage(CreateContext, new JsonCacheFormatter());
		}

		[Fact]
		public async Task GetSet_Gets_IfWasSet()
		{
			const string key = nameof(GetSet_Gets_IfWasSet);

			await _cacheStorage.SetAsync(key, 1);

			Assert.Equal(1, await _cacheStorage.GetAsync<int>(key));
		}

		[Fact]
		public async Task Get_ReturnsBlank_IfNotSet()
		{
			var value = await _cacheStorage.GetAsync<int>(nameof(Get_ReturnsBlank_IfNotSet));

			Assert.False(value.HasValue);
			Assert.Equal(0, value);
		}

		[Fact]
		public async Task  Delete_Deleted_IfWasSet()
		{
			const string key = nameof(Delete_Deleted_IfWasSet);

			await _cacheStorage.SetAsync(key, 1);
			await _cacheStorage.DropAsync(key);

			Assert.False((await _cacheStorage.GetAsync<int>(key)).HasValue);
		}

		[Fact]
		public async Task  Delete_NoErrors_IfNotSet()
		{
			const string key = nameof(Delete_NoErrors_IfNotSet);

			await _cacheStorage.DropAsync(key);

			Assert.False((await _cacheStorage.GetAsync<int>(key)).HasValue);
		}

		private DataConnection CreateContext()
		{
			var context = new DataConnection(new SQLiteDataProvider(), $"Data Source={_dbFileName};");

			context.EnsureTableExists<CacheEntry>();

			return context;
		}

		public void Dispose()
		{
			File.Delete(_dbFileName);
		}
	}
}