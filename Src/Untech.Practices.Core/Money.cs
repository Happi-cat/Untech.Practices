using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.Practices
{
	[DataContract]
	public class Money : ValueObject
	{
		public Money(double amount, Currency currency)
		{
			Amount = amount;
			Currency = currency;
		}

		[DataMember]
		public double Amount { get; private set; }

		[DataMember]
		public Currency Currency { get; private set; }

		protected override IEnumerable<object> GetEquatableProperties()
		{
			yield return Amount;
			yield return Currency;
		}
	}
}