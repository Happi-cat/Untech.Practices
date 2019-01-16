using System.Runtime.Serialization;
using MyBudgetPlan.Domain.MonthLogs;
using NodaTime;
using Untech.Practices;

namespace MyBudgetPlan.Domain.Transactions
{
	[DataContract]
	public class Transaction : BudgetLogEntry
	{
		private Transaction()
		{
		}

		public Transaction(CreateTransaction request)
			: this(0, request.Log, request.CategoryKey, request.When, request.Amount, request.Description)
		{
			Raise(new MonthLogChanged(request.Log, request.When));
		}

		public Transaction(int key, BudgetLogType log, string categoryKey, LocalDate when, Money amount, string description = null)
			: base(key, log)
		{
			CategoryKey = categoryKey;
			When = when;
			Amount = amount;
			Description = description;
		}


		[DataMember]
		public LocalDate When { get; private set; }

		public void Update(UpdateTransaction request)
		{
			UpdateCategory(request.CategoryKey);
			UpdateAmount(request.Amount);
			UpdateDescription(request.Description);
		}

		public void UpdateAmount(Money amount)
		{
			bool shouldRaise = Key == 0 || !Amount.Equals(amount);
			Amount = amount;

			if (shouldRaise) Raise(new MonthLogChanged(Log, When));
		}

		public void UpdateDescription(string description)
		{
			Description = description;
		}

		public void UpdateCategory(string categoryKey)
		{
			string oldValue = CategoryKey;
			CategoryKey = categoryKey;

			bool shouldRaise = Key != 0 && oldValue != categoryKey;
			if (shouldRaise) Raise(new MonthLogChanged(Log, When));
		}
	}
}