using System;
using System.Linq.Expressions;

namespace Untech.Practices.Repos
{
	public class AdHocSpecification<T> : ISpecification<T>
	{
		private readonly Func<T, bool> _compiledExpression;

		public AdHocSpecification(Expression<Func<T, bool>> expression)
		{
			Guard.CheckNotNull("value", expression);

			UnderlyingExpression = expression;
			_compiledExpression = expression.Compile();
		}

		public Expression<Func<T, bool>> UnderlyingExpression { get; }

		public bool IsSatisfiedBy(T entity)
		{
			return _compiledExpression(entity);
		}
	}
}