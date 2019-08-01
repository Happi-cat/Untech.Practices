using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Pipeline
{
	/// <summary>
	///     Defines a pipeline step that will be triggered after <see cref="IRequestHandler{TIn,TOut}" /> or
	///     <see cref="IRequestHandler{TIn,TOut}" />
	/// </summary>
	/// <typeparam name="TRequest">CQRS request type.</typeparam>
	/// <typeparam name="TResponse">CQRS response type.</typeparam>
	public interface IRequestPostProcessor<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		/// <summary>
		///     Post process <paramref name="request" /> and <see cref="response" />.
		/// </summary>
		/// <param name="request">CQRS request.</param>
		/// <param name="response"><see cref="IRequest{TResult}" /> execution response.</param>
		void PostProcess(IRequestHandler<TRequest, TResponse> handler, TRequest request, TResponse response);
	}
}