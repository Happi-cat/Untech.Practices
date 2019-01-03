using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.Actual
{
	[DataContract]
	public class DeleteActualIncome : ICommand
	{
		private DeleteActualIncome()
		{

		}

		public DeleteActualIncome(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}