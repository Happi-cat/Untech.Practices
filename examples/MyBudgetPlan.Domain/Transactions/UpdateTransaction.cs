using System.Runtime.Serialization;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.Transactions
{
	[DataContract]
	public class UpdateTransaction : ICommand<Transaction>
	{
		private UpdateTransaction()
		{

		}

		public UpdateTransaction(int key, string categoryKey, Money amount)
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