using System.Runtime.Serialization;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.Forecasts
{
	[DataContract]
	public class CreateForecast : ICommand<Forecast>
	{
		private CreateForecast()
		{

		}

		public CreateForecast(BudgetLogType log, string categoryKey, YearMonth when, Money amount)
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
		public YearMonth When { get; private set; }

		[DataMember]
		public Money Amount { get; private set; }

		[DataMember]
		public string Description { get; set; }
	}
}