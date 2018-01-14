using System.Collections.Generic;

namespace Untech.Practices.Repos
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	public interface IReadOnlyRepository<TEntity>
	{
		/// <summary>
		/// Finds the one.
		/// </summary>
		/// <param name="specification">The specification.</param>
		/// <returns></returns>
		TEntity FindOne(ISpecification<TEntity> specification);

		/// <summary>
		/// Finds the one.
		/// </summary>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="specification">The specification.</param>
		/// <param name="projection">The projection.</param>
		/// <returns></returns>
		TResult FindOne<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection);

		/// <summary>
		/// Finds the specified specification.
		/// </summary>
		/// <param name="specification">The specification.</param>
		/// <returns></returns>
		IReadOnlyList<TEntity> Find(ISpecification<TEntity> specification);

		/// <summary>
		/// Finds the specified specification.
		/// </summary>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="specification">The specification.</param>
		/// <param name="projection">The projection.</param>
		/// <returns></returns>
		IReadOnlyList<TResult> Find<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection);
	}
}