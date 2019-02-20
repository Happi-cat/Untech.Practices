using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.Practices.CQRS.Handlers;

namespace Untech.Practices.CQRS.Pipeline
{
	/// <summary>
	///     Defines a pipeline step that will be triggered after <see cref="IRequestHandler{TIn,TOut}" /> or
	///     <see cref="IRequestHandler{TIn,TOut}" />
	/// </summary>
	/// <typeparam name="TRequest">CQRS request type.</typeparam>
	/// <typeparam name="TResponse">CQRS response type.</typeparam>
	public interface IPipelinePostProcessor<in TRequest, in TResponse>
	{
		/// <summary>
		///     Post process <paramref name="request" /> and <see cref="response" />.
		/// </summary>
		/// <param name="request">CQRS request.</param>
		/// <param name="response"><see cref="IRequest{TResult}" /> execution response.</param>
		/// :w
		void Process(TRequest request, TResponse response);
	}

	public interface IPipeline<TRequest, TResponse>
	{
		TResponse Invoke(TRequest request, Func<TResponse> next);
	}

	public interface IAsyncPipeline<TRequest, TResponse>
	{
		Task<TResponse> InvokeAsync(TRequest request, Func<TResponse> next, CancellationToken cancellationToken);
	}
}