using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Dispatching
{
	/// <summary>
	///     Represents interface of a class that dispatches CQRS requests to handlers and returns execution results.
	/// </summary>
	/// <remarks>
	///     <para>
	///         Supports <see cref="IQuery{TResult}" />, <see cref="ICommand{TResponse}" />, <see cref="IEvent" />
	///         CQRS requests.
	///     </para>
	/// </remarks>
	public interface IDispatcher : IQueryDispatcher, IEventDispatcher
	{
		/// <summary>
		///     Processes asynchronously the incoming <paramref name="command" /> and returns execution result.
		/// </summary>
		/// <typeparam name="TResult">The type of result.</typeparam>
		/// <param name="command">The command to be processed.</param>
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		/// <returns>Execution result.</returns>
		Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken);
	}
}