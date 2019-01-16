using System;
using System.Runtime.Serialization;
using MyBudgetPlan.Domain.MonthLogs;
using Untech.Practices;

namespace MyBudgetPlan.Domain.Forecasts
{
	[DataContract]
	public class Forecast : BudgetLogEntry
	{
		private Forecast()
		{
		}

		public Forecast(CreateForecast request)
			: this(0, request.Log, request.CategoryKey, request.When, request.Amount, request.Description)
		{
			Raise(new MonthLogChanged(request.Log, request.When));
		}

		public Forecast(int key, BudgetLogType log, string categoryKey, YearMonth when, Money amount, string description = null)
			: base(key, log)
		{
			CategoryKey = categoryKey;
			When = when;
			Amount = amount;
			Description = description;
		}

		[DataMember]
		public YearMonth When { get; private set; }

		public void Update(UpdateForecast request)
		{
			UpdateCategory(request.CategoryKey);
			UpdateAmount(request.Amount);
			UpdateDescription(request.Description);
		}

		public void UpdateAmount(Money amount)
		{
			if (amount.Amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));

			Amount = amount;
			Raise(new MonthLogChanged(Log, When));
		}

		public void UpdateCategory(string categoryKey)
		{
			string oldValue = CategoryKey;
			CategoryKey = categoryKey;

			bool shouldRaise = Key != 0 && oldValue != categoryKey;
			if (shouldRaise) Raise(new MonthLogChanged(Log, When));
		}

		public void UpdateDescription(string description)
		{
			Description = description;
		}
	}
}