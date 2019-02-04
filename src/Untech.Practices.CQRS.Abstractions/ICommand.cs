namespace Untech.Practices.CQRS
{
	/// <summary>
	///     Used as a marker for commands with a response.
	/// </summary>
	/// <typeparam name="TResult">The type of result from the handler.</typeparam>
	public interface ICommand<out TResult> : IRequest<TResult>
	{
	}

	/// <summary>
	///     Used as a marker for commands without a response.
	/// </summary>
	public interface ICommand : ICommand<Nothing>
	{
	}
}