using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.Forecasts
{
	[DataContract]
	public class DeleteForecast : ICommand
	{
		private DeleteForecast()
		{

		}

		public DeleteForecast(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}