using MyBudgetPlan.Domain.Forecasts;
using MyBudgetPlan.Domain.Transactions;
using NodaTime;
using Untech.Practices;
using Untech.Practices.UserContext;

namespace MyBudgetPlan.Infrastructure.Data
{
	public struct BudgetLogDaoMapper : IBudgetLogDaoMapper<Transaction>,
		IBudgetLogDaoMapper<Forecast>
	{
		public Transaction FromDao(IUserContext userContext, BudgetLogEntryDao<Transaction> dao)
		{
			return new Transaction(dao.Key,
				dao.Log,
				dao.Category,
				LocalDate.FromDateTime(dao.When),
				new Money(dao.Amount, new Currency(dao.Currency, dao.Currency)),
				dao.Description);
		}

		public BudgetLogEntryDao<Transaction> ToDao(IUserContext userContext, Transaction entity)
		{
			return new BudgetLogEntryDao<Transaction>(entity.Key,
				userContext.UserKey,
				entity.Log,
				entity.When.ToDateTimeUnspecified(),
				entity.Amount)
			{
				Category = entity.CategoryKey,
				Description = entity.Description
			};
		}

		public Forecast FromDao(IUserContext userContext, BudgetLogEntryDao<Forecast> dao)
		{
			return new Forecast(dao.Key,
				dao.Log,
				dao.Category,
				LocalDate.FromDateTime(dao.When),
				new Money(dao.Amount, new Currency(dao.Currency, dao.Currency)),
				dao.Description);
		}

		public BudgetLogEntryDao<Forecast> ToDao(IUserContext userContext, Forecast entity)
		{
			return new BudgetLogEntryDao<Forecast>(entity.Key,
				userContext.UserKey,
				entity.Log,
				((LocalDate)entity.When).ToDateTimeUnspecified(),
				entity.Amount)
			{
				Category = entity.CategoryKey,
				Description = entity.Description
			};
		}
	}
}