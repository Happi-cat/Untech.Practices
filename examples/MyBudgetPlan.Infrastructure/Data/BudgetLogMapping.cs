using LinqToDB.Mapping;
using MyBudgetPlan.Domain;
using MyBudgetPlan.Domain.ExpenseLog.Actual;
using MyBudgetPlan.Domain.ExpenseLog.Forecast;
using MyBudgetPlan.Domain.IncomeLog.Actual;
using MyBudgetPlan.Domain.IncomeLog.Forecast;

namespace MyBudgetPlan.Infrastructure.Data
{
	public class BudgetLogMappingSchema : MappingSchema
	{
		private  const string SchemaName = "BudgetPlan";

		public BudgetLogMappingSchema()
		{
			FluentMappingBuilder b = GetFluentMappingBuilder();

			ConfigureIncome<ActualIncome>(b, "ActualIncome");
			ConfigureIncome<ProjectedIncome>(b, "ProjectedIncome");
			ConfigureExpense<ActualExpense>(b, "ActualExpense");
			ConfigureExpense<ProjectedExpense>(b, "ProjectedExpense");
		}

		private PropertyMappingBuilder<BudgetLogEntryDao<T>> ConfigureIncome<T>(
			FluentMappingBuilder b,
			 string tableName)
			where T : BudgetLogEntry
		{
			return ConfigureBase<T>(b, tableName);
		}

		private PropertyMappingBuilder<BudgetLogEntryDao<T>> ConfigureExpense<T>(
			FluentMappingBuilder b,
			string tableName)
			where T : BudgetLogEntry
		{
			return ConfigureBase<T>(b, tableName)
				.Property(n => n.Category);
		}

		private PropertyMappingBuilder<BudgetLogEntryDao<T>> ConfigureBase<T>(
			FluentMappingBuilder b,
			string tableName)
			where T : BudgetLogEntry
		{
			return b.Entity<BudgetLogEntryDao<T>>().HasTableName(tableName).HasSchemaName(SchemaName)
				.Property(n => n.Key).IsPrimaryKey().IsIdentity()
				.Property(n => n.UserKey)
				.Property(n => n.When)
				.Property(n => n.Currency)
				.Property(n => n.Amount)
				.Property(n => n.Description).IsNullable();
		}
	}
}