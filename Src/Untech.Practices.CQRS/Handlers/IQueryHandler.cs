namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Defines a handler for a query.
	/// </summary>
	/// <typeparam name="TIn">The type of query to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public interface IQueryHandler<in TIn, out TOut>
		where TIn : IQuery<TOut>
	{
		/// <summary>
		/// Fetches query.
		/// </summary>
		/// <param name="query">Query to fetch.</param>
		/// <returns></returns>
		TOut Fetch(TIn query);
	}
}