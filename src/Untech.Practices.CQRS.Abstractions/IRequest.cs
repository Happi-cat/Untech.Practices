namespace Untech.Practices.CQRS
{
	/// <summary>
	///     Used as a base interface for <see cref="IQuery{TResult}" /> and <see cref="ICommand{TResult}" />.
	///     Shouldn't be inherited by request classes directly.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IRequest<out TResult>
	{
	}
}