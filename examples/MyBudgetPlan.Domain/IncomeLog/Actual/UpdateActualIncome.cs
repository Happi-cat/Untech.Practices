using System.Runtime.Serialization;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.Actual
{
	[DataContract]
	public class UpdateActualIncome : ICommand<ActualIncome>
	{
		private UpdateActualIncome()
		{

		}

		public UpdateActualIncome(int key, string categoryKey, Money amount)
		{
			Key = key;
			CategoryKey = categoryKey;
			Amount = amount;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string CategoryKey { get; private set; }

		[DataMember]
		public Money Amount { get; private set; }

		[DataMember]
		public string Description { get; set; }
	}
}