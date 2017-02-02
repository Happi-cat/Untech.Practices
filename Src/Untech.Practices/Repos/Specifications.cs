namespace Untech.Practices.Repos
{
	public static class Specification
	{
		public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
		{
			return new AndSpecification<T>(left, right);
		}

		public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
		{
			return new OrSpecification<T>(left, right);
		}

		public static ISpecification<T> Not<T>(ISpecification<T> left)
		{
			return new NotSpecification<T>(left);
		}

	}
}