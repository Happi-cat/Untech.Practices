using System.Linq;

namespace Untech.Practices.Repos.Queryable
{
	public interface IReadOnlyRepository<TEntity>
	{
		IQueryable<TEntity> GetAll();
	}
}