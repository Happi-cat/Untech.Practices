using System.Runtime.Serialization;
using NodaTime;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.Transactions
{
	[DataContract]
	public class CreateTransaction : ICommand<Transaction>
	{
		private CreateTransaction()
		{

		}

		public CreateTransaction(BudgetLogType log, string categoryKey, LocalDate when, Money amount)
		{
			Log = log;
			CategoryKey = categoryKey;
			When = when;
			Amount = amount;
		}

		[DataMember]
		public BudgetLogType Log { get; private set; }

		[DataMember]
		public string CategoryKey { get; private set; }

		[DataMember]
		public LocalDate When { get; private set; }

		[DataMember]
		public Money Amount { get; private set; }

		[DataMember]
		public string Description { get; set; }
	}
}