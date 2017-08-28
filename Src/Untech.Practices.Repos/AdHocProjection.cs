using System;
using System.Linq.Expressions;

namespace Untech.Practices.Repos
{
	public class AdHocProjection<TEntity, TResult> : IProjection<TEntity, TResult>
	{
		private readonly Func<TEntity, TResult> _compiledExpression;

		public AdHocProjection(Expression<Func<TEntity, TResult>> expression)
		{
			UnderlyingExpression = expression ?? throw new ArgumentNullException(nameof(expression));
			_compiledExpression = expression.Compile();
		}

		public Expression<Func<TEntity, TResult>> UnderlyingExpression { get; }

		public TResult Project(TEntity entity)
		{
			return _compiledExpression(entity);
		}
	}
}