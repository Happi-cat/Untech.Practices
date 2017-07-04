using System.Threading;
using System.Threading.Tasks;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Defines an async handler for a command.
	/// </summary>
	/// <typeparam name="TIn">The type of command to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public interface ICommandAsyncHandler<in TIn, TOut>
		where TIn : ICommand<TOut>
	{
		/// <summary>
		/// Handles command asynchronously and returns result.
		/// </summary>
		/// <param name="command">Command to be handled.</param>
		/// <param name="cancellationToken">The token that used for propagation notification that task should be canceled.</param>
		/// <returns></returns>
		Task<TOut> ProcessAsync(TIn command, CancellationToken cancellationToken);
	}
}