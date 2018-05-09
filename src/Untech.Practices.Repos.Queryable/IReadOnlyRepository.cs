using System.Linq;

namespace Untech.Practices.Repos.Queryable
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	public interface IReadOnlyRepository<TEntity>
	{
		/// <summary>
		/// Gets all items via <see cref="IQueryable{TEntity}"/>.
		/// </summary>
		/// <returns></returns>
		IQueryable<TEntity> GetAll();
	}
}