using System;
using System.Linq.Expressions;

namespace Untech.Practices.Repos
{
	public class AdHocSpecification<T> : Specification<T>
	{
		private readonly Func<T, bool> _compiledExpression;

		public AdHocSpecification(Expression<Func<T, bool>> expression)
		{
			UnderlyingExpression = expression ?? throw new ArgumentNullException(nameof(expression));
			_compiledExpression = expression.Compile();
		}

		public override Expression<Func<T, bool>> UnderlyingExpression { get; }

		public override bool IsSatisfiedBy(T entity)
		{
			return _compiledExpression(entity);
		}
	}
}