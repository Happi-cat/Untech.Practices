namespace Untech.Practices.CQRS.Handlers
{
	/// <summary>
	/// Defines a handler for a request.
	/// </summary>
	/// <typeparam name="TIn">The type of request to be handled.</typeparam>
	/// <typeparam name="TOut">The type of result from the handler.</typeparam>
	public interface IRequestHandler<in TIn, out TOut>
		where TIn : IRequest<TOut>
	{
		/// <summary>
		/// Handles request and returns result.
		/// </summary>
		/// <param name="request">Request to be handled.</param>
		/// <returns></returns>
		TOut Handle(TIn request);
	}
}