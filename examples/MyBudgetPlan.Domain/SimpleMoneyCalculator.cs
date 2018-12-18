using System.Collections.Generic;
using System.Linq;
using Untech.Practices;

namespace MyBudgetPlan.Domain
{
	public class SimpleMoneyCalculator : IMoneyCalculator
	{
		public Money Sum(IEnumerable<Money> values)
		{
			return values.Aggregate(Sum);
		}

		public Money Sum(Money left, Money right)
		{
			if (left.Currency.Equals(right.Currency))
			{
				return new Money(left.Amount + right.Amount, left.Currency);
			}

			throw new System.NotSupportedException();
		}

		public Money Minus(Money left, Money right)
		{
			if (left.Currency.Equals(right.Currency))
			{
				return new Money(left.Amount - right.Amount, left.Currency);
			}

			throw new System.NotSupportedException();
		}
	}
}