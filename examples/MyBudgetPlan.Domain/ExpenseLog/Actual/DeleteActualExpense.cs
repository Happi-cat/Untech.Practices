using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.ExpenseLog.Actual
{
	[DataContract]
	public class DeleteActualExpense : ICommand
	{
		private DeleteActualExpense()
		{

		}

		public DeleteActualExpense(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}