using System.Collections.Generic;
using Untech.Practices.CQRS;

namespace MyParcels.Domain
{
	public class ParcelsQuery : IQuery<IEnumerable<Parcel>>
	{

	}
}