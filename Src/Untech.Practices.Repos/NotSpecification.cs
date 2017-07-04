using System;
using System.Linq;
using System.Linq.Expressions;

namespace Untech.Practices.Repos
{
	internal sealed class NotSpecification<T> : ISpecification<T>
	{
		private readonly ISpecification<T> _inner;


		public NotSpecification(ISpecification<T> inner)
		{
			_inner = inner;
		}

		public Expression<Func<T, bool>> UnderlyingExpression
		{
			get
			{
				var innerExpression = _inner.UnderlyingExpression;

				var andExpression = Expression.Not(innerExpression.Body);

				return Expression.Lambda<Func<T, bool>>(andExpression, innerExpression.Parameters.Single());
			}
		}

		public bool IsSatisfiedBy(T obj)
		{
			return !_inner.IsSatisfiedBy(obj);
		}
	}
}