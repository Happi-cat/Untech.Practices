using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyParcels.Domain
{
	[DataContract]
	public class CreateOrUpdateParcel : ICommand<Parcel>
	{
		private CreateOrUpdateParcel()
		{

		}

		public CreateOrUpdateParcel(string trackingNumber)
		{
			TrackingNumber = trackingNumber;
		}

		[DataMember]
		public string TrackingNumber { get; private set; }

		[DataMember]
		public string Description { get; set; }
	}
}