using System.Runtime.Serialization;
using NodaTime;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.Forecast
{
	[DataContract]
	public class CreateProjectedIncome : ICommand<ProjectedIncome>
	{
		private CreateProjectedIncome()
		{

		}

		public CreateProjectedIncome(string categoryKey, YearMonth when, Money amount)
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