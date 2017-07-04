using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Defines an asynchronous handler for a query.
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
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		/// <returns></returns>
		Task<TOut> FetchAsync(TIn query, CancellationToken cancellationToken);
	}
}