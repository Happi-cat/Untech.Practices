using System;
using System.Text.Json.Serialization;
using LinqToDB.Mapping;

namespace Untech.Practices.DataStorage.Cache.Linq2Db
{
	[Table("Entries", Schema = "cache")]
	public class CacheEntry
	{
		private CacheEntry()
		{
		}

		public CacheEntry(string key, object value)
		{
			Key = key ?? throw new ArgumentNullException(nameof(key));
			Json = JsonSerializer.ToString(value);
			When = DateTime.UtcNow;
		}

		[Column(CanBeNull = false), PrimaryKey]
		public string Key { get; private set; }

		[Column(CanBeNull = true)]
		public string Json { get; private set; }

		[Column(CanBeNull = false)]
		public DateTime When { get; private set; }

		public T GetValue<T>()
		{
			return JsonSerializer.Parse<T>(Json);
		}
	}
}