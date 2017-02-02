using System.Threading.Tasks;
using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Defines an async handler for a query.
	/// </summary>
	/// <typeparam name="TIn">The type of query to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public interface IQueryAsyncHandler<in TIn, TOut> 
		where TIn : IQuery<TOut>
	{
		/// <summary>
		/// Fetches query asynchronously.
		/// </summary>
		/// <param name="query">Query to fetch.</param>
		/// <returns></returns>
		Task<TOut> FetchAsync(TIn query);
	}
}