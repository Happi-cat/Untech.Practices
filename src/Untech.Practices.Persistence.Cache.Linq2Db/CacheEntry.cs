using System;
using LinqToDB.Mapping;

namespace Untech.Practices.Persistence.Cache.Linq2Db
{
	[Table("Entries", Schema = "cache")]
	public class CacheEntry
	{
		private CacheEntry()
		{
		}

		public CacheEntry(string key, string value)
		{
			Key = key ?? throw new ArgumentNullException(nameof(key));
			Value = value;
			When = DateTime.UtcNow;
		}

		[Column(CanBeNull = false), PrimaryKey]
		public string Key { get; private set; }

		[Column(CanBeNull = true)]
		public string Value { get; private set; }

		[Column(CanBeNull = false)]
		public DateTime When { get; private set; }
	}
}
