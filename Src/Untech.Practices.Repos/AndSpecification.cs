using System;
using System.Linq;
using System.Linq.Expressions;

namespace Untech.Practices.Repos
{
	internal sealed class AndSpecification<T> : ISpecification<T>
	{
		private readonly ISpecification<T> _left;
		private readonly ISpecification<T> _right;

		public AndSpecification(ISpecification<T> left, ISpecification<T> right)
		{
			_right = right;
			_left = left;
		}

		public Expression<Func<T, bool>> UnderlyingExpression
		{
			get
			{
				var leftExpression = _left.UnderlyingExpression;
				var rightExpression = _right.UnderlyingExpression;

				var andExpression = Expression.AndAlso(leftExpression.Body, rightExpression.Body);

				return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters.Single());
			}
		}

		public bool IsSatisfiedBy(T obj)
		{
			return _right.IsSatisfiedBy(obj) && _left.IsSatisfiedBy(obj);
		}
	}
}