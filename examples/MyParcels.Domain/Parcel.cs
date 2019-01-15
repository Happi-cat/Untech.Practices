using System.Runtime.Serialization;
using Untech.Practices.DataStorage;

namespace MyParcels.Domain
{
	[DataContract]
	public class Parcel : IAggregateRoot<string>
	{
		private Parcel()
		{

		}

		public Parcel(string trackingNumber, string description = null)
		{
			Key = trackingNumber;
			Description = description;
		}

		[DataMember]
		public string Key { get; private set; }

		[DataMember]
		public string Description { get; private set; }
	}
}