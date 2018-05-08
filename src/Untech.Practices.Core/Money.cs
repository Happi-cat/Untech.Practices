using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.Practices
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Untech.Practices.ValueObject" />
	[DataContract]
	public class Money : ValueObject<Money>
	{
		protected Money() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Money"/> class.
		/// </summary>
		/// <param name="amount">The amount.</param>
		/// <param name="currency">The currency.</param>
		public Money(double amount, Currency currency)
		{
			Amount = amount;
			Currency = currency;
		}

		/// <summary>
		/// Gets the amount.
		/// </summary>
		/// <value>
		/// The amount.
		/// </value>
		[DataMember]
		public double Amount { get; private set; }

		/// <summary>
		/// Gets the currency.
		/// </summary>
		/// <value>
		/// The currency.
		/// </value>
		[DataMember]
		public Currency Currency { get; private set; }

		protected override IEnumerable<object> GetEquatableProperties()
		{
			yield return Amount;
			yield return Currency;
		}
	}
}