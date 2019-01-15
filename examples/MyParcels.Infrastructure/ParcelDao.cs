namespace MyParcels.Infrastructure.Data
{
	public class ParcelDao
	{
		private ParcelDao()
		{

		}

		public ParcelDao(string key, int userKey)
		{
			Key = key;
			UserKey = userKey;
		}

		public string Key { get; }
		public int UserKey { get; }
		public string Description { get; set; }
	}
}