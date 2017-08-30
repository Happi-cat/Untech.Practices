using System;
using System.Linq.Expressions;

namespace Untech.Practices.Repos
{
	public static class Specification
	{
		public static Specification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
		{
			return new AndSpecification<T>(left, right);
		}

		public static Specification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
		{
			return new OrSpecification<T>(left, right);
		}

		public static Specification<T> Not<T>(ISpecification<T> left)
		{
			return new NotSpecification<T>(left);
		}
	}

	public abstract class Specification<T> : ISpecification<T>
	{
		public abstract Expression<Func<T, bool>> UnderlyingExpression { get; }

		public static Specification<T> operator &(Specification<T> left, ISpecification<T> right)
		{
			return new AndSpecification<T>(left, right);
		}

		public static Specification<T> operator |(Specification<T> left, ISpecification<T> right)
		{
			return new OrSpecification<T>(left, right);
		}

		public static Specification<T> operator !(Specification<T> inner)
		{
			return new NotSpecification<T>(inner);
		}

		public abstract bool IsSatisfiedBy(T entity);
	}
}