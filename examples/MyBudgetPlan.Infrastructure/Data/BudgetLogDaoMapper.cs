using MyBudgetPlan.Domain.ExpenseLog.Actual;
using MyBudgetPlan.Domain.ExpenseLog.Forecast;
using MyBudgetPlan.Domain.IncomeLog.Actual;
using MyBudgetPlan.Domain.IncomeLog.Forecast;
using NodaTime;
using Untech.Practices;
using Untech.Practices.UserContext;

namespace MyBudgetPlan.Infrastructure.Data
{
	public struct BudgetLogDaoMapper : IBudgetLogDaoMapper<ActualIncome>,
		IBudgetLogDaoMapper<ProjectedIncome>,
		IBudgetLogDaoMapper<ActualExpense>,
		IBudgetLogDaoMapper<ProjectedExpense>
	{
		public ActualIncome FromDao(IUserContext userContext, BudgetLogEntryDao<ActualIncome> dao)
		{
			return new ActualIncome(dao.Key,
				LocalDate.FromDateTime(dao.When),
				new Money(dao.Amount, new Currency(dao.Currency, dao.Currency)),
				dao.Description);
		}

		public BudgetLogEntryDao<ActualIncome> ToDao(IUserContext userContext, ActualIncome entity)
		{
			return new BudgetLogEntryDao<ActualIncome>(entity.Key,
				userContext.UserKey,
				entity.When.ToDateTimeUnspecified(),
				entity.Amount)
			{
				Description = entity.Description
			};
		}

		public ProjectedIncome FromDao(IUserContext userContext, BudgetLogEntryDao<ProjectedIncome> dao)
		{
			return new ProjectedIncome(dao.Key,
				LocalDate.FromDateTime(dao.When),
				new Money(dao.Amount, new Currency(dao.Currency, dao.Currency)),
				dao.Description);
		}

		public BudgetLogEntryDao<ProjectedIncome> ToDao(IUserContext userContext, ProjectedIncome entity)
		{
			return new BudgetLogEntryDao<ProjectedIncome>(entity.Key,
				userContext.UserKey,
				((LocalDate)entity.When).ToDateTimeUnspecified(),
				entity.Amount)
			{
				Description = entity.Description
			};
		}

		public ActualExpense FromDao(IUserContext userContext, BudgetLogEntryDao<ActualExpense> dao)
		{
			return new ActualExpense(dao.Key,
				dao.Category,
				LocalDate.FromDateTime(dao.When),
				new Money(dao.Amount, new Currency(dao.Currency, dao.Currency)),
				dao.Description);
		}

		public BudgetLogEntryDao<ActualExpense> ToDao(IUserContext userContext, ActualExpense entity)
		{
			return new BudgetLogEntryDao<ActualExpense>(entity.Key,
				userContext.UserKey,
				entity.When.ToDateTimeUnspecified(),
				entity.Amount)
			{
				Category = entity.CategoryKey,
				Description = entity.Description
			};
		}

		public ProjectedExpense FromDao(IUserContext userContext, BudgetLogEntryDao<ProjectedExpense> dao)
		{
			return new ProjectedExpense(dao.Key,
				dao.Category,
				LocalDate.FromDateTime(dao.When),
				new Money(dao.Amount, new Currency(dao.Currency, dao.Currency)),
				dao.Description);
		}

		public BudgetLogEntryDao<ProjectedExpense> ToDao(IUserContext userContext, ProjectedExpense entity)
		{
			return new BudgetLogEntryDao<ProjectedExpense>(entity.Key,
				userContext.UserKey,
				((LocalDate)entity.When).ToDateTimeUnspecified(),
				entity.Amount)
			{
				Category = entity.CategoryKey,
				Description = entity.Description
			};
		}
	}
}