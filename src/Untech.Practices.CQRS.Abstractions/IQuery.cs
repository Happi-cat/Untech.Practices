namespace Untech.Practices.CQRS
{
	/// <summary>
	///     Used as a marker for queries.
	/// </summary>
	/// <typeparam name="TResult">The type of result from the handler.</typeparam>
	public interface IQuery<out TResult> : IRequest<TResult>
	{
	}
}