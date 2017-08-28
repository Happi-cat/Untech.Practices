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

		public double Amount { get; private set; }
		public Currency Currency { get; private set; }

		protected override IEnumerable<object> GetEquatableProperties() 
		{
			yield return Amount;
			yield return Currency;
		}
	}
}