using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyParcels.Domain
{
	[DataContract]
	public class DeleteParcel : ICommand
	{
		public DeleteParcel(string trackingNumber)
		{
			TrackingNumber = trackingNumber;
		}

		[DataMember]
		public string TrackingNumber { get; private set; }
	}
}