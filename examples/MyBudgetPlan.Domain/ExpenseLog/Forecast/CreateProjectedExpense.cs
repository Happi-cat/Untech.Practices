using System.Runtime.Serialization;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Forecast
{
	[DataContract]
	public class CreateProjectedExpense : ICommand<ProjectedExpense>
	{
		private CreateProjectedExpense()
		{

		}

		public CreateProjectedExpense(string categoryKey, YearMonth when, Money amount)
		{
			CategoryKey = categoryKey;
			When = when;
			Amount = amount;
		}

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