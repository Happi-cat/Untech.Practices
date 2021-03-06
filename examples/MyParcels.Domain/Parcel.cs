﻿using System.Runtime.Serialization;
using Untech.Practices.Persistence;

namespace MyParcels.Domain
{
	[DataContract]
	public class Parcel : IHasKey<string>
	{
		private Parcel()
		{

		}

		public Parcel(string trackingNumber, int userKey, string description = null)
		{
			Key = trackingNumber;
			UserKey = userKey;
			Description = description;
		}

		[DataMember]
		public string Key { get; private set; }

		[DataMember]
		public int UserKey { get; private set; }

		[DataMember]
		public string Description { get; private set; }
	}
}
