using LinqToDB.Mapping;
using MyBudgetPlan.Domain;
using MyBudgetPlan.Domain.Forecasts;
using MyBudgetPlan.Domain.Transactions;

namespace MyBudgetPlan.Infrastructure.Data
{
	public class BudgetLogMappingSchema : MappingSchema
	{
		private  const string SchemaName = "BudgetPlan";

		public BudgetLogMappingSchema()
		{
			FluentMappingBuilder b = GetFluentMappingBuilder();

			Configure<Transaction>(b, "Transactions");
			Configure<Forecast>(b, "Forecasts");
		}

		private PropertyMappingBuilder<BudgetLogEntryDao<T>> Configure<T>(
			FluentMappingBuilder b,
			string tableName)
			where T : BudgetLogEntry
		{
			return b.Entity<BudgetLogEntryDao<T>>().HasTableName(tableName).HasSchemaName(SchemaName)
				.Property(n => n.Key).IsPrimaryKey().IsIdentity()
				.Property(n => n.UserKey)
				.Property(n => n.Log)
				.Property(n => n.When)
				.Property(n => n.Currency)
				.Property(n => n.Amount)
				.Property(n => n.Description).IsNullable()
				.Property(n => n.Category);
		}
	}
}