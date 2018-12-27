using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Forecast
{
	[DataContract]
	public class DeleteProjectedExpense : ICommand<ProjectedExpense>
	{
		private DeleteProjectedExpense()
		{

		}

		public DeleteProjectedExpense(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}