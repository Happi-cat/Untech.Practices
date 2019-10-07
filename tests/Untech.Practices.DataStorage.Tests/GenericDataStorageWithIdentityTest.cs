using System;
using System.IO;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using Untech.Practices.DataStorage.Linq2Db;
using Xunit;

namespace Untech.Practices.DataStorage
{
	public class GenericDataStorageWithIdentityTest : IDisposable
	{
		private readonly string _dbFileName;
		private readonly IDataStorage<Entity, int> _dataStorage;

		public GenericDataStorageWithIdentityTest()
		{
			_dbFileName = $"{nameof(GenericDataStorageWithIdentityTest)}.temp.db";
			_dataStorage = new GenericDataStorage<Entity, int>(CreateContext);
		}

		[Fact]
		public async Task CreateAsync_ReturnsObjectWithIncrementedIdentity()
		{
			var created = await _dataStorage.CreateAsync(new Entity());

			Assert.NotEqual(0, created.Key);
		}

		[Fact]
		public async Task UpdateAsync_ReturnsUpdateAsyncObject()
		{
			var created = await _dataStorage.CreateAsync(new Entity());

			created.Value = "new value";

			var updated = await _dataStorage.UpdateAsync(created);

			Assert.Equal(created.Value, updated.Value);
		}

		[Fact]
		public async Task DeleteAsync_Removes()
		{
			var created = await _dataStorage.CreateAsync(new Entity());

			var deleted = await _dataStorage.DeleteAsync(created);

			Assert.True(deleted);
		}

		[Fact]
		public async Task GetAsync_ReturnsObject()
		{
			var created = await _dataStorage.CreateAsync(new Entity());

			var found = await _dataStorage.GetAsync(created.Key);

			Assert.Equal(created.Key, found.Key);
		}

		[Fact]
		public async Task GetAsync_Throws_WhenNotFound()
		{
			await Assert.ThrowsAsync<ItemNotFoundException>(() => _dataStorage.GetAsync(-1));
		}

		[Table("entities")]
		private class Entity : IHasKey
		{
			[Column, PrimaryKey, Identity]
			public int Key { get; set; }

			[Column, Nullable]
			public string Value { get; set; }
		}

		private DataConnection CreateContext()
		{
			var context = new DataConnection(new SQLiteDataProvider(), $"Data Source={_dbFileName};");

			context.EnsureTableExists<Entity>();

			return context;
		}

		public void Dispose()
		{
			File.Delete(_dbFileName);
		}
	}
}