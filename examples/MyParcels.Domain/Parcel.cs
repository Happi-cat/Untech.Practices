using System.Collections.Generic;
using Untech.Practices.DataStorage;

namespace MyParcels.Domain
{
	public class Parcel : IAggregateRoot<string>
	{
		public string Key { get; private set; }

		public string Description { get; private set; }

		public IReadOnlyList<ParcelStatus> Status { get; set; }
	}
}