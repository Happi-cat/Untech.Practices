using Untech.Practices.CQRS;

namespace MyParcels.Domain
{
	public class ParcelTrackingQuery : IQuery<ParcelTracking>
	{
		public ParcelTrackingQuery(string trackingNumber)
		{
			TrackingNumber = trackingNumber;
		}

		public string TrackingNumber { get; private set; }
	}
}