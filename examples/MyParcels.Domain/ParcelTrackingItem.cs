using System;
using System.Runtime.Serialization;

namespace MyParcels.Domain
{
	[DataContract]
	public class ParcelTrackingItem
	{
		private ParcelTrackingItem()
		{

		}

		public ParcelTrackingItem(DateTimeOffset when, string message)
		{
			When = when;
			Message = message;
		}

		[DataMember]
		public DateTimeOffset When { get; private set; }

		[DataMember]
		public string Message { get; private set; }
	}
}