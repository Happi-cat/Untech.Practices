using System.Runtime.Serialization;
using NodaTime;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.Actual
{
	[DataContract]
	public class CreateActualIncome : ICommand<ActualIncome>
	{
		private CreateActualIncome()
		{

		}

		public CreateActualIncome(string categoryKey, LocalDate when, Money amount)
		{
			CategoryKey = categoryKey;
			When = when;
			Amount = amount;
		}

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