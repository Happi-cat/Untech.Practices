using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace MyParcels.Domain
{
	[DataContract]
	public class ParcelTracking
	{
		private ParcelTracking()
		{

		}

		public ParcelTracking(string key, IEnumerable<ParcelTrackingItem> items = null)
		{
			Key = key;
			Items = items?.ToList() ?? new List<ParcelTrackingItem>();
		}

		[DataMember]
		public string Key { get; private set; }

		[DataMember]
		public IReadOnlyList<ParcelTrackingItem> Items { get; private set; }
	}
}