using System.Collections.Generic;

namespace Untech.Practices.Repos
{
	public interface IReadOnlyRepository<TEntity>
	{
		TEntity FindOne(ISpecification<TEntity> specification);

		TResult FindOne<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection);

		IReadOnlyList<TEntity> Find(ISpecification<TEntity> specification);

		IReadOnlyList<TResult> Find<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection);
	}
}