using System;
using System.Linq.Expressions;

namespace Untech.Practices.Repos
{
	public interface ISpecification<T>
	{
		Expression<Func<T, bool>> UnderlyingExpression { get; }

		bool IsSatisfiedBy(T obj);
	}
}