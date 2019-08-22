using System;
using System.Data;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace Untech.Practices.DataStorage
{
	public static class DataConnectionExtensions
	{
		public static void EnsureTableExists<T>(this DataConnection context)
		{
			if (IsTableExists<T>(context))
				return;

			context.CreateTable<T>();
		}

		private static bool IsTableExists<T>(DataConnection context)
		{
			EntityDescriptor entityDescriptor = context.MappingSchema.GetEntityDescriptor(typeof(T));

			return IsTableExists(context, entityDescriptor.TableName);
		}

		private static bool IsTableExists(DataConnection context, string tableName)
		{
			tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));

			IDbCommand command = context.CreateCommand();
			command.CommandText = "SELECT 1 FROM sqlite_master WHERE type='table' AND name=@pName";
			command.CommandType = CommandType.Text;

			IDbDataParameter pName = command.CreateParameter();
			pName.ParameterName = "@pName";
			pName.Value = tableName;

			command.Parameters.Add(pName);

			long result = (long?)command.ExecuteScalar() ?? 0;

			return result == 1;
		}
	}
}