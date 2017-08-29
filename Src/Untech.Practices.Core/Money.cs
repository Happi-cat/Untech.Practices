using System.Collections.Generic;

namespace Untech.Practices
{
	public class Money : ValueObject
	{
		public Money(double amount, Currency currency)
		{
			Amount = amount;
			Currency = currency;
		}

		public double Amount { get;set; }
		public Currency Currency { get;set; }

		protected override IEnumerable<object> GetEquatableProperties() 
		{
			yield return Amount;
			yield return Currency;
		}
	}
}