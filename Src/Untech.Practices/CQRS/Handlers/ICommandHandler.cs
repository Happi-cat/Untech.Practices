using Untech.Practices.CQRS.Requests;

namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Defines a handler for a command.
	/// </summary>
	/// <typeparam name="TIn">The type of command to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public interface ICommandHandler<in TIn, out TOut>
		where TIn : ICommand<TOut>
	{
		/// <summary>
		/// Handles command and returns result.
		/// </summary>
		/// <param name="command">Command to be handled.</param>
		/// <returns></returns>
		TOut Handle(TIn command);
	}
}