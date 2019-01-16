using LinqToDB.Mapping;
using MyParcels.Domain;

namespace MyParcels.Infrastructure
{
	public class MyParcelsMapping : MappingSchema
	{
		public MyParcelsMapping()
		{
			var b = GetFluentMappingBuilder();

			b.Entity<Parcel>().HasTableName("Parcels").HasTableName("MyParcels")
				.Property(n=> n.Key).IsPrimaryKey()
				.Property(n => n.UserKey).IsPrimaryKey()
				.Property(n => n.Description).IsNullable();
		}
	}
}