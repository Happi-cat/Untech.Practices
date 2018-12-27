using System.Runtime.Serialization;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Forecast
{
	[DataContract]
	public class UpdateProjectedExpense : ICommand<ProjectedExpense>
	{
		private UpdateProjectedExpense()
		{

		}

		public UpdateProjectedExpense(int key, string categoryKey, Money amount)
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