using System;
using System.Linq.Expressions;

namespace Untech.Practices.Repos
{
	public interface IProjection<TSource, TResult>
	{
		Expression<Func<TSource, TResult>> UnderlyingExpression { get; }

		TResult Project(TSource obj);
	}
}