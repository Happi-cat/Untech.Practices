using System;

namespace MyParcels.Domain
{
	public class ParcelStatus
	{
		public DateTimeOffset When { get; private set; }

		public string Description { get; private set; }
	}
}