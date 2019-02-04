using System.Collections.Generic;
using Untech.Practices;

namespace MyBudgetPlan.Domain
{
	public interface IMoneyCalculator
	{
		Money Sum(IEnumerable<Money> values);
		Money Sum(Money left, Money right);
		Money Minus(Money left, Money right);
	}
}