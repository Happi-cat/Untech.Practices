using System;
using System.IO;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;
using Untech.Practices.Persistence.Linq2Db;
using Xunit;

namespace Untech.Practices.Persistence
{
	public class GenericDataStorageTest : IDisposable
	{
		private readonly string _dbFileName;
		private readonly IDataStorage<Entity, Guid> _dataStorage;

		public GenericDataStorageTest()
		{
			_dbFileName = $"{nameof(GenericDataStorageTest)}.temp.db";
			_dataStorage = new GenericDataStorage<Entity, Guid>(CreateContext);
		}

		[Fact]
		public async Task CreateAsync_ReturnsObjectWithSameId()
		{
			var initialized = new Entity();

			var created = await _dataStorage.CreateAsync(initialized);

			Assert.Equal(initialized.Key, created.Key);
		}

		[Fact]
		public async Task UpdateAsync_ReturnsUpdatedObject()
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
			await Assert.ThrowsAsync<ItemNotFoundException>(() => _dataStorage.GetAsync(Guid.NewGuid()));
		}

		[Table("entities")]
		private class Entity : IHasKey<Guid>
		{
			[Column, PrimaryKey]
			public Guid Key { get; set; } = Guid.NewGuid();

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
