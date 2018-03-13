using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	/// Represents interface of a class that dispatches CQRS query-requests to handlers and returns execution results.
	/// </summary>
	/// <remarks>
	/// <para>Supports <see cref="IQuery{TResult}"/> CQRS requests.</para>
	/// </remarks>
	public interface IQueryDispatcher
	{
		/// <summary>
		/// Fetches data of type <typeparamref name="TResult"/> for the specified <paramref name="query"/>.
		/// </summary>
		/// <typeparam name="TResult">The type of result.</typeparam>
		/// <param name="query">The query to be processed.</param>
		/// <returns>Matching data.</returns>
		TResult Fetch<TResult>(IQuery<TResult> query);

		/// <summary>
		/// Fetches asynchronously data of type <typeparamref name="TResult"/> for the specified <paramref name="query"/>.
		/// </summary>
		/// <typeparam name="TResult">The type of result.</typeparam>
		/// <param name="query">The query to be processed.</param>
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		/// <returns>Matching data.</returns>
		Task<TResult> FetchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
	}
}