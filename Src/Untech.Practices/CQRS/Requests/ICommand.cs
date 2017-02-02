namespace Untech.Practices.CQRS.Requests
{
	/// <summary>
	/// Used as a marker for commands with a reponse.
	/// </summary>
	/// <typeparam name="TResponse">The type of result from the handler.</typeparam>
	public interface ICommand<TResponse>
	{
	}

	/// <summary>
	/// Used as a marker for commands without a response.
	/// </summary>
	public interface ICommand : ICommand<Unit>
	{
	}
}