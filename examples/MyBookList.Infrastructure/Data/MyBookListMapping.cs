using LinqToDB.Mapping;

namespace MyBookList.Infrastructure.Data
{
	public class MyBookListMapping : MappingSchema
	{
		public MyBookListMapping()
		{
			var b = GetFluentMappingBuilder();

			b.Entity<MyBookDao>().HasTableName("MyBooks").HasSchemaName("MyBookList")
				.Property(n => n.Key).IsPrimaryKey().IsIdentity()
				.Property(n => n.UserKey)
				.Property(n => n.Author)
				.Property(n => n.Title)
				.Property(n => n.BookKey).IsNullable()
				.Property(n => n.Ordering).IsNullable()
				.Property(n => n.Review).IsNullable();
		}
	}
}