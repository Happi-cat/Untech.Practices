using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.IncomeLog.Forecast
{
	[DataContract]
	public class DeleteProjectedIncome : ICommand<ProjectedIncome>
	{
		public DeleteProjectedIncome(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}