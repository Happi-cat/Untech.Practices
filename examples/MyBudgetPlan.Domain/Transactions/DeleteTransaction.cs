using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.Transactions
{
	[DataContract]
	public class DeleteTransaction : ICommand
	{
		private DeleteTransaction()
		{

		}

		public DeleteTransaction(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}