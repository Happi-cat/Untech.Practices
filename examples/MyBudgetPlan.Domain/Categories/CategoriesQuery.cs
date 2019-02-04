using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace MyBudgetPlan.Domain.Categories
{
	[DataContract]
	public class CategoriesQuery : IQuery<IEnumerable<Category>>
	{
	}
}